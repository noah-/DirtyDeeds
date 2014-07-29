﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteMagic;

namespace Itchy
{
    public partial class D2Game
    {
        protected int[] m_ActLevels = new int[]
        {
            1, 40, 75, 103, 109, 137
        };

        public void RevealAct()
        {
            SuspendThreads();

            UnitAny unit;
            if (!GetPlayerUnit(out unit))
            {
                ResumeThreads();
                Log("Failed to reveal act");
                return;
            }

            if (revealedActs.Contains(unit.dwAct))
            {
                ResumeThreads();
                Log("Act {0} is already revealed", unit.dwAct + 1);
                return;
            }

            if (unit.pAct == 0)
            {
                ResumeThreads();
                Log("Failed to reveal act {0}", unit.dwAct + 1);
                return;
            }

            ResumeStormThread();

            var act = pd.Read<Act>(unit.pAct);
            var expCharFlag = pd.ReadUInt(pd.GetModuleAddress("d2client.dll") + D2Client.pExpCharFlag);
            var diff = pd.ReadByte(pd.GetModuleAddress("d2client.dll") + D2Client.pDifficulty);

            var pAct = pd.Call(pd.GetModuleAddress("d2common.dll") + D2Common.LoadAct,
                CallingConventionEx.StdCall,
                unit.dwAct,
                act.dwMapSeed,
                expCharFlag,
                0,
                diff,
                0,
                (uint)m_ActLevels[unit.dwAct],
                pd.GetModuleAddress("d2client.dll") + D2Client.LoadAct_1,
                pd.GetModuleAddress("d2client.dll") + D2Client.LoadAct_2);

            if (pAct == 0)
            {
                ResumeThreads();
                Log("Failed to reveal act");
                return;
            }

            act = pd.Read<Act>(pAct);
            if (act.pMisc == 0)
            {
                ResumeThreads();
                Log("Failed to reveal act");
                return;
            }

            var actMisc = pd.Read<ActMisc>(act.pMisc);
            if (actMisc.pLevelFirst == 0)
            {
                ResumeThreads();
                Log("Failed to reveal act");
                return;
            }

            for (var i = m_ActLevels[unit.dwAct]; i < m_ActLevels[unit.dwAct + 1]; ++i)
            {
                Level lvl = pd.Read<Level>(actMisc.pLevelFirst);
                uint pLevel = 0;
                for (pLevel = actMisc.pLevelFirst; pLevel != 0; pLevel = lvl.pNextLevel)
                {
                    if (pLevel != actMisc.pLevelFirst)
                        lvl = pd.Read<Level>(pLevel);

                    if (lvl.dwLevelNo == (uint)i && lvl.dwPosX > 0)
                        break;
                }

                if (pLevel == 0)
                    pLevel = pd.Call(pd.GetModuleAddress("d2common.dll") + D2Common.GetLevel,
                        CallingConventionEx.FastCall,
                        act.pMisc, (uint)i);
                if (pLevel == 0)
                    continue;

                lvl = pd.Read<Level>(pLevel);
                if (lvl.pRoom2First == 0)
                    pd.Call(pd.GetModuleAddress("d2common.dll") + D2Common.InitLevel,
                        CallingConventionEx.StdCall,
                        pLevel);

                if (lvl.dwLevelNo > 255)
                    continue;

                InitLayer(lvl.dwLevelNo);
                lvl = pd.Read<Level>(pLevel);

                for (var pRoom = lvl.pRoom2First; pRoom != 0; )
                {
                    var room = pd.Read<Room2>(pRoom);

                    var actMisc2 = pd.Read<ActMisc>(lvl.pMisc);
                    var roomData = false;
                    if (room.pRoom1 == 0)
                    {
                        roomData = true;
                        pd.Call(pd.GetModuleAddress("d2common.dll") + D2Common.AddRoomData,
                            CallingConventionEx.ThisCall,
                            0, actMisc2.pAct, lvl.dwLevelNo, room.dwPosX, room.dwPosY, room.pRoom1);
                    }

                    room = pd.Read<Room2>(pRoom);
                    if (room.pRoom1 == 0)
                        continue;

                    var pAutomapLayer = pd.ReadUInt(pd.GetModuleAddress("d2client.dll") + D2Client.pAutoMapLayer);

                    pd.Call(pd.GetModuleAddress("d2client.dll") + D2Client.RevealAutomapRoom,
                        CallingConventionEx.StdCall,
                        room.pRoom1,
                        1,
                        pAutomapLayer);

                    if (roomData)
                        pd.Call(pd.GetModuleAddress("d2common.dll") + D2Common.RemoveRoomData,
                            CallingConventionEx.StdCall,
                            actMisc2.pAct, lvl.dwLevelNo, room.dwPosX, room.dwPosY, room.pRoom1);

                    pRoom = room.pRoom2Next;
                }
            }

            var path = pd.Read<Path>(unit.pPath);
            var room1 = pd.Read<Room1>(path.pRoom1);
            var room2 = pd.Read<Room2>(room1.pRoom2);
            var lev = pd.Read<Level>(room2.pLevel);
            InitLayer(lev.dwLevelNo);
            pd.Call(pd.GetModuleAddress("d2common.dll") + D2Common.UnloadAct,
                CallingConventionEx.StdCall,
                pAct);

            ResumeThreads();

            //PrintGameString("Revealed act", D2Color.Red);

            revealedActs.Add(unit.dwAct);

            Log("Revealed act {0}", unit.dwAct + 1);
        }

        public void InitLayer(uint levelNo)
        {
            var pLayer = pd.Call(pd.GetModuleAddress("d2common.dll") + D2Common.GetLayer,
                CallingConventionEx.FastCall,
                levelNo);
            if (pLayer == 0)
                return;

            var layer = pd.Read<AutomapLayer2>(pLayer);

            pd.Call(pd.GetModuleAddress("d2client.dll") + D2Client.InitAutomapLayer_I,
                CallingConventionEx.Register,
                layer.nLayerNo);
        }
    }
}