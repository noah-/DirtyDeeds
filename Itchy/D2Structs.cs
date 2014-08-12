﻿using System;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Itchy
{
    [StructLayout(LayoutKind.Explicit)]
    public struct UnitAny
    {
        [FieldOffset(0x0)]
        public uint dwType;
        [FieldOffset(0x4)]
        public uint dwTxtFileNo;
        [FieldOffset(0x8)]
        public uint _1;
        [FieldOffset(0xC)]
        public uint dwUnitId;
        [FieldOffset(0x10)]
        public uint dwMode;
        [FieldOffset(0x14)]
        public uint pPlayerData;        // PlayerData*
        [FieldOffset(0x14)]
        public uint pItemData;          // ItemData*
        [FieldOffset(0x14)]
        public uint pMonsterData;       // MonsterData*
        [FieldOffset(0x14)]
        public uint pObjectData;        // ObjectData*
        [FieldOffset(0x18)]
        public uint dwAct;
        [FieldOffset(0x1C)]
        public uint pAct;               // Act*
        [FieldOffset(0x20)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U4)]
        public uint[] dwSeed;
        [FieldOffset(0x28)]
        public uint _2;
        [FieldOffset(0x2C)]
        public uint pPath;              // Path*
        [FieldOffset(0x2C)]
        public uint pItemPath;          // ItemPath*
        [FieldOffset(0x2C)]
        public uint pObjectPath;        // ObjectPath*
        [FieldOffset(0x30)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.U4)]
        public uint[] _3;
        [FieldOffset(0x44)]
        public uint dwGfxFrame;
        [FieldOffset(0x48)]
        public uint dwFrameRemain;
        [FieldOffset(0x4C)]
        public ushort wFrameRate;
        [FieldOffset(0x4E)]
        public ushort _4;
        [FieldOffset(0x50)]
        public uint pGfxUnk;            // BYTE*
        [FieldOffset(0x54)]
        public uint pGfxInfo;           // DWORD*
        [FieldOffset(0x58)]
        public uint _5;
        [FieldOffset(0x5C)]
        public uint pStats;             // StatList*
        [FieldOffset(0x60)]
        public uint pInventory;         // Inventory*
        [FieldOffset(0x64)]
        public uint ptLight;            // Light*
        [FieldOffset(0x68)]
        public uint dwStartLightRadius;
        [FieldOffset(0x6C)]
        public ushort nPl2ShiftIdx;
        [FieldOffset(0x6E)]
        public ushort nUpdateType;
        [FieldOffset(0x70)]
        public uint pUpdateUnit;        // UnitAny* - Used when updating unit.
        [FieldOffset(0x74)]
        public uint pQuestRecord;       // DWORD*
        [FieldOffset(0x78)]
        public uint bSparklyChest;      // bool
        [FieldOffset(0x7C)]
        public uint pTimerArgs;         // DWORD*
        [FieldOffset(0x80)]
        public uint dwSoundSync;
        [FieldOffset(0x84)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U4)]
        public uint[] _6;
        [FieldOffset(0x8C)]
        public ushort wX;
        [FieldOffset(0x8E)]
        public ushort wY;
        [FieldOffset(0x90)]
        public uint _7;
        [FieldOffset(0x94)]
        public uint dwOwnerType;
        [FieldOffset(0x98)]
        public uint dwOwnerId;
        [FieldOffset(0x9C)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U4)]
        public uint[] _8;
        [FieldOffset(0xA4)]
        public uint pOMsg;              // OverheadMsg*
        [FieldOffset(0xA8)]
        public uint pInfo;              // Info*
        [FieldOffset(0xAC)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.U4)]
        public uint[] _9;
        [FieldOffset(0xC4)]
        public uint dwFlags;
        [FieldOffset(0xC8)]
        public uint dwFlags2;
        [FieldOffset(0xCC)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.U4)]
        public uint[] _10;
        [FieldOffset(0xE0)]
        public uint pChangedNext;       // UnitAny*
        [FieldOffset(0xE4)]
        public uint pListNext;          // UnitAny* 0xE4 -> 0xD8
        [FieldOffset(0xE8)]
        public uint pRoomNext;          // UnitAny*

        public bool IsRune()
        {
            if ((UnitType)dwType != UnitType.Item)
                return false;

            return dwTxtFileNo >= 610 && dwTxtFileNo <= 642;
        }

        public uint RuneNumber()
        {
            if (!IsRune())
                return 0;

            return dwTxtFileNo - 610 + 1;
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    struct PlayerData
    {
        [FieldOffset(0x0)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x10, ArraySubType = UnmanagedType.U1)]
        public byte[] szName;
        [FieldOffset(0x10)]
        uint pNormalQuest;              // QuestInfo*
        [FieldOffset(0x14)]
        public uint pNightmareQuest;    // QuestInfo*
        [FieldOffset(0x18)]
        public uint pHellQuest;         // QuestInfo*
        [FieldOffset(0x1C)]
        public uint pNormalWaypoint;    // Waypoint*
        [FieldOffset(0x20)]
        public uint pNightmareWaypoint; // Waypoint*
        [FieldOffset(0x24)]
        public uint pHellWaypoint;      // Waypoint*
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct Inventory
    {
        [FieldOffset(0x0)]
        public uint dwSignature;
        [FieldOffset(0x04)]
        public uint bGame1C;            // BYTE*
        [FieldOffset(0x08)]
        public uint pOwner;             // UnitAny*
        [FieldOffset(0x0C)]
        public uint pFirstItem;         // UnitAny*
        [FieldOffset(0x10)]
        public uint pLastItem;          // UnitAny*
        [FieldOffset(0x14)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U4)]
        public uint[] _1;
        [FieldOffset(0x1C)]
        public uint dwLeftItemUid;
        [FieldOffset(0x20)]
        public uint pCursorItem;        // UnitAny*
        [FieldOffset(0x24)]
        public uint dwOwnerId;
        [FieldOffset(0x28)]
        public uint dwItemCount;
    }

    /*[StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ItemData
    {
        public uint dwQuality;          // 0x00
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U4)]
        public uint[] dwSeed;           // 0x04
        public uint dwItemFlags;        // 0x0C 1 = Owned by player, 0xFFFFFFFF = Not owned
        public uint dwFingerPrint;      // 0x10 Initial seed
        public uint _1;                 // 0x14 CommandFlags?
        public uint dwFlags;            // 0x18
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U4)]
        public uint[] _2;               // 0x1C
        public uint dwActionStamp;      // 0x24 Changes when an item is changed
        public uint dwFileIndex;        // 0x28 Data file index UniqueItems.txt etc.
        public uint dwItemLevel;        // 0x2C
        public ushort wItemFormat;      // 0x30
        public ushort wRarePrefix;      // 0x32
        public ushort wRareSuffix;      // 0x34
        public ushort wAutoPrefix;      // 0x36
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.U2)]
        public ushort[] wMagicPrefix;   // 0x38
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.U2)]
        public ushort[] wMagicSuffix;   // 0x3E
        public byte BodyLocation;       // 0x44 Not always cleared
        public byte ItemLocation;       // 0x45 Non-body/belt location (Body/Belt == 0xFF)
        public ushort _4;               // 0x46
        public byte bEarLevel;          // 0x48
        public byte bInvGfxIdx;         // 0x49
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 18, ArraySubType=UnmanagedType.U1)]
        public char[] szPlayerName;     // 0x4A Personalized / Ear name
        public uint pOwnerInventory;    // Inventory* 0x5C Socketed Items owner Inv 0x5A
        public uint _10;                // 0x60
        public uint pNextInvItem;       // UnitAny 0x64 Next item in socketed item if OwnerInventory is set
        public byte GameLocation;       // 0x68 Location per docs.d2bs.org (unit.location)
        public byte NodePage;           // 0x69 Actual location, this is the most reliable by far
        public ushort _12;              // 0x6A
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12, ArraySubType = UnmanagedType.U2)]
        public ushort[] _13;            // 0x6C
        public uint pOwner;             // UnitAny* 0x84
    }*/

    /*[StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ItemData
    {
        public uint dwQuality;				//0x00
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U4)]
        public uint[] _1;					//0x04
        public uint dwItemFlags;				//0x0C 1 = Owned by player, 0xFFFFFFFF = Not owned
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U4)]
        public uint[] _2;					//0x10
        public uint dwFlags;					//0x18
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.U4)]
        public uint[] _3;					//0x1C
        public uint dwQuality2;				//0x28
        public uint dwItemLevel;				//0x2C
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U4)]
        public uint[] _4;					//0x30
        public ushort wPrefix;					//0x38
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U2)]
        public ushort[] _5;						//0x3A
        public ushort wSuffix;					//0x3E
        public uint _6;						//0x40
        public byte BodyLocation;				//0x44
        public byte ItemLocation;				//0x45 Non-body/belt location (Body/Belt == 0xFF)
        public byte _7;						//0x46
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x15, ArraySubType = UnmanagedType.U2)]
        public byte[] _8;
        public uint pOwnerInventory;		// Inventory 0x5C +
        public uint _10;						//0x60
        public uint pNextInvItem;			// UnitAny 0x64
        public byte GameLocation;			//0x68
        public byte NodePage;					//0x69 Actual location, this is the most reliable by far
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x9A, ArraySubType = UnmanagedType.U1)]
        public byte[] _12;						//0x6A
        public uint pOwner;				// UnitAny 0x84 0x104
    }*/

    [StructLayout(LayoutKind.Explicit)]
    public struct ItemData
    {
        [FieldOffset(0)]
        public uint dwQuality;
        [FieldOffset(0x18)]
        public uint dwFlags;
        [FieldOffset(0x2C)]
        public uint dwItemLevel;
        [FieldOffset(0x44)]
        public uint BodyLocation;
        [FieldOffset(0x45)]
        public uint ItemLocation;
        [FieldOffset(0x69)]
        public byte NodePage;
        [FieldOffset(0x70)]
        public uint pNextInvItem;
        //[FieldOffset(0x104)]
        //public uint pOwner;
    }


    /*[StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ItemTxt
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x40, ArraySubType=UnmanagedType.U1)]
        public byte[] szName2;          // 0x00
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType=UnmanagedType.U1)]
        public byte[] szCode;           // 0x40
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x70, ArraySubType=UnmanagedType.U1)]
        public byte[] _2;               // 0x84
        public ushort nLocaleTxtNo;     // 0xF4
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x19, ArraySubType=UnmanagedType.U1)]
        public byte[] _2a;              // 0xF7
        public byte xSize;              // 0xFC
        public byte ySize;              // 0xFD
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 13, ArraySubType=UnmanagedType.U1)]
        public byte[] _2b;              // 0xFE
        public byte nType;              // 0x11E
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0xD, ArraySubType=UnmanagedType.U1)]
        public byte[] _3;               // 0x11F
        public byte fQuest;             // 0x12A
    }*/

    [StructLayout(LayoutKind.Explicit)]
    public struct ItemTxt
    {
        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
        public string szFlippyFile;     // 0x00
        [FieldOffset(0x20)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
        public string szInvFile;        // 0x20
        [FieldOffset(0x40)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
        public string szUniqueInvFile;  // 0x40
        [FieldOffset(0x60)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
        public string szSetInvFile;     // 0x60
        [FieldOffset(0x80)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U1)]
        public byte[] szCode;           // 0x40
        [FieldOffset(0xFC)]
        public byte rarity;
        [FieldOffset(0x10F)]
        public byte xSize;              // 0x10F
        [FieldOffset(0x110)]
        public byte ySize;              // 0x110

        public string GetCode()
        {
            return Encoding.ASCII.GetString(szCode).Replace(" ", "");
        }

        public uint GetDwCode()
        {
            return BitConverter.ToUInt32(szCode, 0) & 0xFFF;
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct Act
    {
        [FieldOffset(0x0)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.U4)]
        public uint[] _1;
        [FieldOffset(0xC)]
        public uint dwMapSeed;
        [FieldOffset(0x10)]
        public uint pRoom1;             // Room1*
        [FieldOffset(0x14)]
        public uint dwAct;
        [FieldOffset(0x18)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12, ArraySubType = UnmanagedType.U4)]
        public uint[] _2;
        [FieldOffset(0x48)]
        public uint pMisc;              // ActMisc*
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct ActMisc
    {
        [FieldOffset(0x0)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 37, ArraySubType = UnmanagedType.U4)]
        public uint[] _1;
        [FieldOffset(0x94)]
        public uint dwStaffTombLevel;
        [FieldOffset(0x98)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 245, ArraySubType = UnmanagedType.U4)]
        public uint[] _2;
        [FieldOffset(0x46C)]
        public uint pAct;               // Act*
        [FieldOffset(0x470)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.U4)]
        public uint[] _3;
        [FieldOffset(0x47C)]
        public uint pLevelFirst;        // Level*
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct Level
    {
        [FieldOffset(0x0)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U4)]
        public uint[] _1;
        [FieldOffset(0x10)]
        public uint pRoom2First;        // Room2*
        [FieldOffset(0x14)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U4)]
        public uint[] _2;
        [FieldOffset(0x1C)]
        public uint dwPosX;
        [FieldOffset(0x20)]
        public uint dwPosY;
        [FieldOffset(0x24)]
        public uint dwSizeX;
        [FieldOffset(0x28)]
        public uint dwSizeY;
        [FieldOffset(0x2C)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 96, ArraySubType = UnmanagedType.U4)]
        public uint[] _3;
        [FieldOffset(0x1AC)]
        public uint pNextLevel;         // Level*
        [FieldOffset(0x1B0)]
        public uint _4;
        [FieldOffset(0x1B4)]
        public uint pMisc;              // ActMisc*
        [FieldOffset(0x1BC)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.U4)]
        public uint[] _5;
        [FieldOffset(0x1D0)]
        public uint dwLevelNo;
        [FieldOffset(0x1D4)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.U4)]
        public uint[] _6;

        [FieldOffset(0x1E0)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9, ArraySubType = UnmanagedType.U4)]
        public uint[] RoomCenterX;
        [FieldOffset(0x1E0)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9, ArraySubType = UnmanagedType.U4)]
        public uint[] WarpX;

        [FieldOffset(0x204)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9, ArraySubType = UnmanagedType.U4)]
        public uint[] RoomCenterY;
        [FieldOffset(0x204)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9, ArraySubType = UnmanagedType.U4)]
        public uint[] WarpY;

        [FieldOffset(0x228)]
        public uint dwRoomEntries;

        public bool IsTown() { return dwLevelNo == 1 || dwLevelNo == 40 || dwLevelNo == 75 || dwLevelNo == 103 || dwLevelNo == 109; }
        public bool IsUberTristram() { return dwLevelNo == 136; }
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct Room1
    {
        [FieldOffset(0x0)]
        public uint pRoomsNear;         // Room1**
        [FieldOffset(0x04)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.U4)]
        public uint[] _1;
        [FieldOffset(0x10)]
        public uint pRoom2;             // Room2*
        [FieldOffset(0x14)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.U4)]
        public uint[] _2;
        [FieldOffset(0x20)]
        public uint Coll;               // CollMap*
        [FieldOffset(0x24)]
        public uint dwRoomsNear;
        [FieldOffset(0x28)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9, ArraySubType = UnmanagedType.U4)]
        public uint[] _3;
        [FieldOffset(0x4C)]
        public uint dwXStart;
        [FieldOffset(0x50)]
        public uint dwYStart;
        [FieldOffset(0x54)]
        public uint dwXSize;
        [FieldOffset(0x58)]
        public uint dwYSize;
        [FieldOffset(0x5C)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.U4)]
        public uint[] _4;
        [FieldOffset(0x74)]
        public uint pUnitFirst;         // UnitAny*
        [FieldOffset(0x78)]
        public uint _5;
        [FieldOffset(0x7C)]
        public uint pRoomNext;          // Room1*
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct Room2
    {
        [FieldOffset(0x0)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U4)]
        public uint[] _1;               //0x00
        [FieldOffset(0x8)]
        public uint pRoom2Near;         // Room2**
        [FieldOffset(0xC)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5, ArraySubType = UnmanagedType.U4)]
        public uint[] _2;
        [FieldOffset(0x20)]
        public uint pType2Info;         // Type2Info*
        [FieldOffset(0x24)]
        public uint pRoom2Next;         // Room2*
        [FieldOffset(0x28)]
        public uint dwRoomFlags;
        [FieldOffset(0x2C)]
        public uint dwRoomsNear;
        [FieldOffset(0x30)]
        public uint pRoom1;             // Room1*
        [FieldOffset(0x34)]
        public uint dwPosX;
        [FieldOffset(0x38)]
        public uint dwPosY;
        [FieldOffset(0x3C)]
        public uint dwSizeX;
        [FieldOffset(0x40)]
        public uint dwSizeY;
        [FieldOffset(0x44)]
        public uint _3;
        [FieldOffset(0x48)]
        public uint dwPresetType;
        [FieldOffset(0x4C)]
        public uint pRoomTiles;         // RoomTile*
        [FieldOffset(0x50)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U4)]
        public uint[] _4;
        [FieldOffset(0x58)]
        public uint pLevel;             // Level*
        [FieldOffset(0x5C)]
        public uint pPreset;            // PresetUnit*
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct Type2Info
    {
        [FieldOffset(0x0)]
        public uint dwRoomNumber;
        [FieldOffset(0x4)]
        public uint _1;
        [FieldOffset(0x8)]
        public uint pdwSubNumber;       // DWORD*
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct AutomapLayer
    {
        [FieldOffset(0x0)]
        public uint nLayerNo;
        [FieldOffset(0x04)]
        public uint fSaved;
        [FieldOffset(0x08)]
        public uint pFloors;            // AutomapCell*
        [FieldOffset(0x0C)]
        public uint pWalls;             // AutomapCell*
        [FieldOffset(0x10)]
        public uint pObjects;           // AutomapCell*
        [FieldOffset(0x14)]
        public uint pExtras;            // AutomapCell*
        [FieldOffset(0x18)]
        public uint pNextLayer;         // AutomapLayer*
    };

    [StructLayout(LayoutKind.Explicit)]
    public struct AutomapLayer2
    {
        [FieldOffset(0x0)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U4)]
        public uint[] _1;
        [FieldOffset(0x08)]
        public uint nLayerNo;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct Path
    {
        [FieldOffset(0x0)]
        public ushort xOffset;
        [FieldOffset(0x2)]
        public ushort xPos;
        [FieldOffset(0x4)]
        public ushort yOffset;
        [FieldOffset(0x6)]
        public ushort yPos;
        [FieldOffset(0x8)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U4)]
        public uint[] _1;
        [FieldOffset(0x10)]
        public ushort xTarget;
        [FieldOffset(0x12)]
        public ushort yTarget;
        [FieldOffset(0x14)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U4)]
        public uint[] _2;
        [FieldOffset(0x1C)]
        public uint pRoom1;             // Room1*
        [FieldOffset(0x20)]
        public uint pRoomUnk;           // Room1*
        [FieldOffset(0x24)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.U4)]
        public uint[] _3;
        [FieldOffset(0x30)]
        public uint pUnit;              // UnitAny*
        [FieldOffset(0x34)]
        public uint dwFlags;
        [FieldOffset(0x38)]
        public uint _4;
        [FieldOffset(0x3C)]
        public uint dwPathType;
        [FieldOffset(0x40)]
        public uint dwPrevPathType;
        [FieldOffset(0x44)]
        public uint dwUnitSize;
        [FieldOffset(0x48)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U4)]
        public uint[] _5;
        [FieldOffset(0x58)]
        public uint pTargetUnit;        // UnitAny*
        [FieldOffset(0x5C)]
        public uint dwTargetType;
        [FieldOffset(0x60)]
        public uint dwTargetId;
        [FieldOffset(0x64)]
        public byte bDirection;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RosterUnit
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x10)]
        string szName;                  // 0x00
        public uint dwUnitId;           // 0x10
        public uint dwPartyLife;        // 0x14
        public uint _1;                 // 0x18
        public uint dwClassId;          // 0x1C
        public ushort wLevel;           // 0x20
        public ushort wPartyId;         // 0x22
        public uint dwLevelId;          // 0x24
        public uint Xpos;               // 0x28
        public uint Ypos;               // 0x2C
        public uint dwPartyFlags;       // 0x30
        public uint _5;                 // 0x34 BYTE*
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11, ArraySubType = UnmanagedType.U4)]
        public uint[] _6;               // 0x38
        public ushort _7;               // 0x64
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x10)]
        string szName2;                 // 0x66
        public ushort _8;               // 0x76
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = UnmanagedType.U4)]
        public uint[] _9;               // 0x78
        public uint pNext;              // 0x80 RosterUnit*
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PresetUnit
    {
        public uint _1;                 // 0x00
        public uint dwTxtFileNo;        // 0x04
        public uint dwPosX;             // 0x08
        public uint pPresetNext;        // 0x0C PresetUnit
        public uint _3;                 // 0x10
        public uint dwType;             // 0x14
        public uint dwPosY;             // 0x18
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ObjectTxt
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x40, ArraySubType = UnmanagedType.U1)]
        public byte[] szName;           // 0x00
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x40, ArraySubType = UnmanagedType.U1)]
        public byte[] wszName;          // 0x40
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U1)]
        public byte[] _1;               // 0xC0
        public byte nSelectable0;       // 0xC4
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x87, ArraySubType = UnmanagedType.U1)]
        public byte[] _2;               // 0xC5
        public byte nOrientation;       // 0x14C
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x19, ArraySubType = UnmanagedType.U1)]
        public byte[] _2b;              // 0x14D
        public byte nSubClass;          // 0x166
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x11, ArraySubType = UnmanagedType.U1)]
        public byte[] _3;               // 0x167
        public byte nParm0;             // 0x178
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x39, ArraySubType = UnmanagedType.U1)]
        public byte[] _4;               // 0x179
        public byte nPopulateFn;        // 0x1B2
        public byte nOperateFn;         // 0x1B3
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.U1)]
        public byte[] _5;               // 0x1B4
        public uint nAutoMap;           // 0x1BB
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AutomapCell
    {
        public uint fSaved;             // 0x00
        public ushort nCellNo;          // 0x04
        public ushort xPixel;           // 0x06
        public ushort yPixel;           // 0x08
        public ushort wWeight;          // 0x0A
        public uint pLess;              // 0x0C AutomapCell*
        public uint pMore;              // 0x10 AutomapCell*
    };
}
