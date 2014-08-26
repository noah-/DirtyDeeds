﻿using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WhiteMagic;

namespace Itchy
{
    //TODO: 2 unknowns left... are there imbue and craft / transmute action types ?
    public enum ItemActionType
    {
        AddToGround = 0,
        GroundToCursor = 1,			// only sent if item goes to cursor (GS packet 0x0A removes item from ground)
        DropToGround = 2,
        OnGround = 3,
        PutInContainer = 4,
        RemoveFromContainer = 5,
        Equip = 6,
        /// <summary>
        /// Sent for the equipped item when changing from a two handed weapon to a single handed weapon or vice versa.
        /// The item must be equiped on the "empty" hand or a regular SwapBodyItem will be sent instead.
        /// Empty hand meaning left hand if currently wearing a two handed weapon or the empty hand if wearing a single hand item.
        /// The result will be the new item being equipped and the old going to cursor.
        /// </summary>
        IndirectlySwapBodyItem = 7,
        Unequip = 8,
        SwapBodyItem = 9,
        AddQuantity = 0x0A,
        AddToShop = 0x0B,
        RemoveFromShop = 0x0C,
        SwapInContainer = 0x0D,
        PutInBelt = 0x0E,
        RemoveFromBelt = 0x0F,
        SwapInBelt = 0x10,
        /// <summary>
        /// Sent for the secondary hand's item going to inventory when changing from a dual item setup to a two handed weapon.
        /// </summary>
        AutoUnequip = 0x11,
        RemoveFromHireling = 0x12,	// sent along with a 9d 08 packet... Also Item on cursor when entering game ?? MiscToCursor??
        ItemInSocket = 0x13,
        UNKNOWN1 = 0x14,
        UpdateStats = 0x15,			// put item in socket; for each potion that drops in belt when lower one is removed...
        UNKNOWN2 = 0x16,
        WeaponSwitch = 0x17,
    }

    /// <summary>
    /// This is used by GS Packet 0x9C - 0x9D, and is pretty weird...
    /// </summary>
    public enum ItemCategory
    {
        Helm = 0,
        Armor = 1,
        /// <summary>
        /// Most weapons, including Crossbows
        /// </summary>
        Weapon = 5,
        /// <summary>
        /// Bows (not crossbows), sometimes shield (if equiped in LeftHand?)
        /// </summary>
        Weapon2 = 6,
        /// <summary>
        /// Shields can some somtimes be Weapon2...
        /// </summary>
        Shield = 7,
        /// <summary>
        /// Class specific items !?
        /// </summary>
        Special = 10,
        /// <summary>
        /// BaseMiscItems and gloves, boots...
        /// </summary>
        Misc = 16,
    }

    [Flags]
    public enum ItemFlag : uint
    {
        None = 0,
        Equipped = 1,
        // UNKNOWN		= 2,
        // UNKNOWN		= 4,
        InSocket = 8,
        Identified = 0x10,		 // Not undentified, really...
        x20 = 0x20,		 // Has to do with aura / state change !?
        SwitchedIn = 0x40,
        SwitchedOut = 0x80,
        Broken = 0x100,
        // UNKNOWN		= 0x200,
        Potion = 0x400,	 // Set for Mana, Healing and Rejuvenation potions, but not always !?!
        Socketed = 0x800,
        // UNKNOWN		= 0x1000,	 // WasPickedUp ? NOT !
        InStore = 0x2000,	 // Not always set when in store !?
        NotInSocket = 0x4000,	 // Illegal Equip ?
        // UNKNOWN		= 0x8000,
        Ear = 0x10000,	 // Has different packet structure
        StartItem = 0x20000,	 // Item character started with (meaning it's worthless)
        //UNKNOWN		= 0x40000,
        //UNKNOWN		= 0x80000,
        //UNKNOWN		= 0x100000,
        SimpleItem = 0x200000,	 // No ILevel
        Ethereal = 0x400000,
        Any = 0x800000,	 // Which means ??
        Personalized = 0x1000000,
        Gamble = 0x2000000, // Item a town folk is offering for gambling (same purpose as SimpleItem: no ILevel+ info)
        Runeword = 0x4000000,
        x8000000 = 0x8000000, // InducesTempStatusChange ??
        MASK = 0xFE36DF9, // Known / used values
    }

    public enum ItemContainer
    {
        Equipment = 0x00,	// Player or Merc Equipment
        Ground = 0x01,
        Inventory = 0x02,
        TraderOffer = 0x04,
        ForTrade = 0x06,
        Cube = 0x08,
        Stash = 0x0A,
        // Not a buffer... if (buffer == Equipement && Location == EquipmentLocation.NotApplicable)
        Belt = 0x0C,
        // Not a buffer... if (buffer == Equipement && destination == ItemDestination.Item)
        Item = 0x0E,
        //NPC buffers are flagged with 0x80 so they are different
        ArmorTab = 0x82,
        WeaponTab1 = 0x84,
        WeaponTab2 = 0x86,
        MiscTab = 0x88,
        //ArmorTabBottom	= 0x83, // Buffer merged with ArmorTab
        //WeaponTab1Bottom	= 0x85, // Buffer merged with WeaponTab1
        //MiscTabBottom		= 0x89, // Buffer merged with WeaponTab2
    }

    public enum EquipmentLocation
    {
        NotApplicable = 0,
        Helm = 1,
        Amulet = 2,
        Armor = 3,
        RightHand = 4,
        LeftHand = 5,
        RightHandRing = 6,
        LeftHandRing = 7,
        Belt = 8,
        Boots = 9,
        Gloves = 10,
        RightHandSwitch = 11,
        LeftHandSwitch = 12,
    }

    public class ByteConverter
    {
        public static int GetBits(byte[] bytes, ref int offset, int length)
        {
            offset += length;
            return GetBits(bytes, offset - length, length);
        }

        public static int GetBits(byte[] bytes, int offset, int length)
        {
            int bytesLen = bytes.Length * 8;
            if (offset < 0 || !(offset < bytesLen))
                throw new ArgumentOutOfRangeException("offset");
            if (length < 1 || length > 32 || (offset + length > bytesLen))
                throw new ArgumentOutOfRangeException("length");

            int result = 0;
            int bytePos = offset / 8;
            int bitPos = offset % 8;
            byte b = bytes[bytePos];
            int byteBits;
            int totBits = 0;

            while (length > 0)
            {
                if (bitPos == 8)
                {
                    b = bytes[++bytePos];
                    bitPos = 0;
                }
                byteBits = Math.Min(length, 8 - bitPos);
                result |= GetBits(b, bitPos, byteBits) << totBits;
                bitPos += byteBits;
                totBits += byteBits;
                length -= byteBits;
            }
            return result;
        }

        public static int GetBits(byte val, int offset, int length)
        {
            //TODO: support BE: if (bigEndian) offset = 8 - offset - length;
            return ((val & (((1 << (offset + length)) - 1) & ~((1 << offset) - 1))) >> offset);
        }
    }

    public class ItemActionInfo
    {
        public ItemActionType action = ItemActionType.AddToGround;

        public ItemFlag flags = ItemFlag.None;

        public uint uid = 0;
        public string code = "";
        public ItemQuality quality = ItemQuality.Normal;

        public int x = 0;
        public int y = 0;
        public byte iLvl = 0;
        public int sockets = 0;

        public int goldCount = 0;

        public bool IsEth { get { return flags.HasFlag(ItemFlag.Ethereal); } }

        public double DistanceSq(double x, double y)
        {
            return Math.Pow(this.x - x, 2) + Math.Pow(this.y - y, 2);
        }

        public ItemInfo info = null;
        public List<ItemProcessingInfo> processingInfo = null;

        public DateTime pickDate;
        public uint pickTryCount = 0;
    }

    public partial class D2Game
    {
        public Pickit Pickit { get { return pickit; } }
        protected Pickit pickit = null;

        public ItemActionInfo ReadItemAction(byte[] data)
        {
            var i = new ItemActionInfo();

            i.action = (ItemActionType)data[1];
            var category = (ItemCategory)data[3];
            i.uid = BitConverter.ToUInt32(data, 4);

            var pOffset = data[0] == 0x9D ? 13 : 8;

            i.flags = (ItemFlag)BitConverter.ToUInt32(data, pOffset);

            var version = data[pOffset += 4];

            ++pOffset;

            //Log("Uid {0} action {1}", i.uid, i.action);

            if (i.action != ItemActionType.AddToGround &&
                i.action != ItemActionType.DropToGround &&
                i.action != ItemActionType.OnGround)
                return null;

            i.x = BitConverter.ToUInt16(data, pOffset);
            i.y = BitConverter.ToUInt16(data, pOffset += 2);

            i.x = (i.x + 131072) / 32;
            i.y = (i.y + 131072) / 32;
            pOffset += 2;

            //TODO: Unknown bit
            //pOffset = pOffset * 8 + 1;

            pOffset = pOffset * 8 - 32 + 5;
            i.x = ByteConverter.GetBits(data, ref pOffset, 16);
            i.y = ByteConverter.GetBits(data, ref pOffset, 16);

            //var tmp = ByteConverter.GetBits(data, ref pOffset, 4);
            var container = ItemContainer.Ground;

            if (i.flags.HasFlag(ItemFlag.Ear))
            {
                return null;
                /*var charClass = ByteConverter.GetBits(data, ref pOffset, 3);
                var level = (ushort)ByteConverter.GetBits(data, ref pOffset, 7);
                var builder = new System.Text.StringBuilder();
                int mChar;
                while ((mChar = ByteConverter.GetBits(data, ref pOffset, 7)) != 0)
                    builder.Append((char)mChar);
                var name = builder.ToString();
                return i;*/
            }

            i.code =  String.Concat(
                (char)ByteConverter.GetBits(data, ref pOffset, 8),
                (char)ByteConverter.GetBits(data, ref pOffset, 8),
                (char)ByteConverter.GetBits(data, ref pOffset, 8));

            pOffset += 4;

            if (i.code == "gld")
            {
                pOffset += 4;

                if (ByteConverter.GetBits(data, ref pOffset, 1) == 1)
                    i.goldCount = ByteConverter.GetBits(data, ref pOffset, 20);
                else
                    i.goldCount = ByteConverter.GetBits(data, ref pOffset, 18);

                return i;
            }

            var location = EquipmentLocation.NotApplicable;

            // Buffer to container mapping (sanitize NPC tabs IDs and coords, changed belt location to X, Y, etc.)
            if (i.flags.HasFlag(ItemFlag.InStore) // Flag is not always set for shop items !?!
                || i.action == ItemActionType.AddToShop || i.action == ItemActionType.RemoveFromShop)
            {
                int buff = (int)container | 0x80;
                if (i.y < 2 && (buff == 0x83 || buff == 0x85 || buff == 0x89))
                {
                    buff -= 1;
                    i.y += 8;
                }
                container = (ItemContainer)buff;
            }
            else if (container == ItemContainer.Equipment)
            {
                if (location == EquipmentLocation.NotApplicable)
                {
                    if ((i.flags & ItemFlag.InSocket) != 0)
                        container = ItemContainer.Item;
                    else
                    {
                        container = ItemContainer.Belt;
                        i.y = i.x / 4;
                        i.x = i.x % 4;
                    }
                }
                else
                {
                    // Hides them in dump... not needed anyway
                    i.y = -1;
                    i.x = -1;
                }
            }

            // TODO: I don't know what this is (always 2 ??)
            var unknown1 = (byte)ByteConverter.GetBits(data, ref pOffset, 4);

            // Used Sockets : 3
            byte usedSockets = 0;
            if (i.flags.HasFlag(ItemFlag.Socketed))
                usedSockets = (byte)ByteConverter.GetBits(data, pOffset, 3);
            pOffset += 3;

            // Ends here if Simple or Gamble Item
            if ((i.flags & ItemFlag.SimpleItem) != 0
            || (i.flags & ItemFlag.Gamble) != 0)
            {
                return i;
            }

            // ILevel : 7
            i.iLvl = (byte)ByteConverter.GetBits(data, ref pOffset, 7);

            // Quality : 4
            i.quality = (ItemQuality)ByteConverter.GetBits(data, ref pOffset, 4);

            // Graphic : 1 : 3
            if (ByteConverter.GetBits(data, ref pOffset, 1) == 1)
               ByteConverter.GetBits(data, ref pOffset, 3);

            //TODO: ClassInfo : 1 : 11
            if (ByteConverter.GetBits(data, ref pOffset, 1) == 1)
                ByteConverter.GetBits(data, ref pOffset, 11);

            // Quality information
            if (i.flags.HasFlag(ItemFlag.Identified))
            {
                switch (i.quality)
                {
                    case ItemQuality.Inferior:
                        /*this.prefix = new ItemAffix(ItemAffixType.InferiorPrefix,
                            ByteConverter.GetBits(data, ref pOffset, 3));*/
                        ByteConverter.GetBits(data, ref pOffset, 3);
                        break;
                    case ItemQuality.Superior:
                        //this.prefix = new ItemAffix(ItemAffixType.SuperiorPrefix, 0);
                        //TODO: quality type	
                        //	00 = AR
                        //	01 = Max Dmg
                        //	02 = AC
                        //	03 = AR + Max Dmg
                        //	04 = Durability
                        //	05 = Durability + AR
                        //	06 = Durability + Max Dmg
                        //	07 = Durability + AC 
                        pOffset += 3;
                        break;
                    case ItemQuality.Magic:
                        /*this.prefix = new ItemAffix(ItemAffixType.MagicPrefix,
                            ByteConverter.GetBits(data, ref pOffset, 11));
                        this.suffix = new ItemAffix(ItemAffixType.MagicSuffix,
                            ByteConverter.GetBits(data, ref pOffset, 11));*/
                        ByteConverter.GetBits(data, ref pOffset, 11);
                        ByteConverter.GetBits(data, ref pOffset, 11);
                        break;
                    case ItemQuality.Rare:
                    case ItemQuality.Craft:
                        /*this.prefix = new ItemAffix(ItemAffixType.RarePrefix,
                            ByteConverter.GetBits(data, ref pOffset, 8));
                        this.suffix = new ItemAffix(ItemAffixType.RareSuffix,
                            ByteConverter.GetBits(data, ref pOffset, 8));*/
                        ByteConverter.GetBits(data, ref pOffset, 8);
                        ByteConverter.GetBits(data, ref pOffset, 8);
                        for (int j = 0; j < 3; ++j)
                        {
                            if (ByteConverter.GetBits(data, ref pOffset, 1) == 1)
                                ByteConverter.GetBits(data, ref pOffset, 11);
                                //this.magicPrefixes[i] = new ItemAffix(ItemAffixType.MagicPrefix,
                                    //ByteConverter.GetBits(data, ref pOffset, 11));
                            if (ByteConverter.GetBits(data, ref pOffset, 1) == 1)
                                ByteConverter.GetBits(data, ref pOffset, 11);
                                //this.magicSuffixes[i] = new ItemAffix(ItemAffixType.MagicSuffix,
                                    //ByteConverter.GetBits(data, ref pOffset, 11));
                        }
                        break;
                    case ItemQuality.Set:
                        //this.setItem = BaseSetItem.Get(ByteConverter.GetBits(data, ref pOffset, 12));
                        ByteConverter.GetBits(data, ref pOffset, 12);
                        break;

                    case ItemQuality.Unique:
                        if (i.code != "std") // TODO: add UniqueItem entry to parse mod (req lvl 90)
                            ByteConverter.GetBits(data, ref pOffset, 12);
                            //this.uniqueItem = BaseUniqueItem.Get(ByteConverter.GetBits(data, ref pOffset, 12));
                        break;
                }
            }

            // Personalized Name : 7 * (NULLSTRING Length)
            if (i.flags.HasFlag(ItemFlag.Personalized))
            {
                System.Text.StringBuilder builder = new System.Text.StringBuilder();
                int mChar;
                while ((mChar = ByteConverter.GetBits(data, ref pOffset, 7)) != 0)
                    builder.Append((char)mChar);
                var personalizedName = builder.ToString();
            }

            // Runeword Info : 16
            if (i.flags.HasFlag(ItemFlag.Runeword))
            {
                //HACK: this is probably wrong, but works for all the runewords I tested so far...
                //TODO: Need to test cases where runewordUnknown != 5 and where ID is around 100
                //TODO: remove these fields once testing is done
                var runewordID = ByteConverter.GetBits(data, ref pOffset, 12);
                var runewordUnknown = ByteConverter.GetBits(data, ref pOffset, 4);
            }

            var itemInfo = ItemStorage.GetInfo(i.code);
            if (itemInfo == null)
                return null;

            if (itemInfo.BodyLocation == ItemBodyLocation.Armor &&
                itemInfo.ArmorType != ItemArmorType.Amulet &&
                itemInfo.ArmorType != ItemArmorType.Ring)
            {
                var defense = ByteConverter.GetBits(data, ref pOffset, 11) - 10;
            }

            var maxDurability = 0;
            var currentDurability = 0;
            if (itemInfo.BodyLocation == ItemBodyLocation.Armor &&
                itemInfo.ArmorType != ItemArmorType.Amulet &&
                itemInfo.ArmorType != ItemArmorType.Ring ||
                itemInfo.BodyLocation == ItemBodyLocation.Weapon)
            {
                maxDurability = ByteConverter.GetBits(data, ref pOffset, 8);

                // 0 durability means indestructible, but only found on old items (having the "zod bug"...)
                if (maxDurability > 0)
                {
                    currentDurability = ByteConverter.GetBits(data, ref pOffset, 8);

                    //TODO: unknown. Indestructible bit ??
                    ByteConverter.GetBits(data, ref pOffset, 1);
                }
            }

            // Total Sockets : 4
            if (i.flags.HasFlag(ItemFlag.Socketed))
                i.sockets = ByteConverter.GetBits(data, ref pOffset, 4);

            return i;
        }

        public bool ItemActionHandler(byte[] data)
        {
            var i = ReadItemAction(data);
            if (i == null)
                return true;

            switch (i.action)
            {
                case ItemActionType.Equip:
                case ItemActionType.IndirectlySwapBodyItem:
                case ItemActionType.Unequip:
                case ItemActionType.SwapBodyItem:
                    break;
            }

            if (!Settings.ReceivePacketHack.ItemTracker.Enabled)
                return true;

            if (pickit == null/* || pickit.fullInventory*/)
                return true;

            var itemInfo = ItemStorage.GetInfo(i.code);
            if (itemInfo == null)
                return true;

            var matchingEntries = ItemProcessingSettings.GetMatches(itemInfo, (uint)i.sockets, i.IsEth, i.quality);

            i.info = itemInfo;
            i.processingInfo = matchingEntries.Where(it => it.Pick).ToList();

            var log = matchingEntries.Where(it => it.Log).Count() != 0;
            var pick = Settings.ReceivePacketHack.ItemTracker.EnablePickit && (!IsInTown() || Settings.ReceivePacketHack.ItemTracker.TownPick) ? i.processingInfo.Count != 0 : false;

            if (i.quality == ItemQuality.Set && Settings.ReceivePacketHack.ItemTracker.LogSets
                || i.quality == ItemQuality.Unique && Settings.ReceivePacketHack.ItemTracker.LogUniques
                || itemInfo.IsRune() && Settings.ReceivePacketHack.ItemTracker.LogRunes
                || log && Settings.ReceivePacketHack.ItemTracker.LogItems)
            {
                var message = "Dropped ";

                var addsocks = false;
                var addilvl = false;

                var color = i.quality.GetColor();
                switch (i.quality)
                {
                    case ItemQuality.Unique:
                    {
                        if (itemInfo.PossibleUniques.Count == 1)
                            message += itemInfo.PossibleUniques[0];
                        else if (itemInfo.IsBijou())
                            message += itemInfo.Name;
                        else
                        {
                            message += itemInfo.Name + " (";
                            for (int j = 0; j < itemInfo.PossibleUniques.Count; ++j)
                            {
                                message += itemInfo.PossibleUniques[j];
                                if (j != itemInfo.PossibleUniques.Count - 1)
                                    message += ", ";
                            }
                            message += ")";
                        }
                        break;
                    }
                    case ItemQuality.Set:
                    {
                        if (itemInfo.PossibleSets.Count == 1)
                            message += itemInfo.PossibleSets[0];
                        else if (itemInfo.IsBijou())
                            message += itemInfo.Name;
                        else
                        {
                            message += itemInfo.Name + " (";
                            for (int j = 0; j < itemInfo.PossibleSets.Count; ++j)
                            {
                                message += itemInfo.PossibleSets[j];
                                if (j != itemInfo.PossibleSets.Count - 1)
                                    message += ", ";
                            }
                            message += ")";
                        }
                        break;
                    }
                    case ItemQuality.Normal:
                    {
                        message += itemInfo.Name;
                        if (itemInfo.IsRune())
                        {
                            color = Color.MediumPurple;
                            message += " (" + itemInfo.RuneNumber().ToString() + ")";
                        }
                        else
                        {
                            addsocks = true;
                            addilvl = true;
                        }
                        break;
                    }
                    case ItemQuality.Superior:
                    {
                        message += "Superior " + itemInfo.Name;
                        addsocks = true;
                        addilvl = true;
                        break;
                    }
                    default:
                    {
                        message += itemInfo.Name;
                        break;
                    }
                }

                if (addsocks && i.sockets != 0)
                    message += " (" + i.sockets.ToString() + ")";
                if (addilvl && i.iLvl > 1)
                    message += " (L" + i.iLvl.ToString() + ")";
                if (i.IsEth)
                    message += " (Eth)";

                Log(color, message);
            }

            if (pick)
            {
                Task.Factory.StartNew(() => pickit.AddPendingItem(i));
                //Log("Added {0} {1} to pickit", i.code, i.uid);
            }

            return true;
        }

        public void ItemGoneHandler(byte[] data)
        {
            if (pickit == null/* || pickit.fullInventory*/)
                return;

            var unitType = (UnitType)data[1];
            if (unitType != UnitType.Item)
                return;

            var uid = BitConverter.ToUInt32(data, 2);
            Task.Factory.StartNew(() => pickit.RemoveItem(uid));
            //Log("Removing uid {0} from pickit", uid);
        }

        public void OnRelocaton()
        {
            if (pickit == null || pickit.fullInventory)
                return;

            pickit.ProcessPicks(true);
        }
    }
}