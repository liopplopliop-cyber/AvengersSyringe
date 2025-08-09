using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

#pragma warning disable CS0618 // Type or member is obsolete

/*

BIG LIST OF GOOD TO KNOW CODING SHIT FOR PPG

Limbs:

0 Head,
1 UpperBody,
2 MiddleBody,
3 LowerBody,
4 UpperLegFront,
5 LowerLegFront,
6 FootFront,
7 UpperLeg,
8 LowerLeg,
9 Foot,
10 UpperArmFront,
11 LowerArmFront,
12 UpperArm,
13 LowerArm

Poses:

0 Rest,
1 Protective,
2 Flailing,
3 Stumbling,
4 Swimming,
5 WrithingInPain,
6 Walking,
7 Sitting,
8 Flat,
9 BrainDamage,
10 None
Then it loops (etc 11 = Rest)
*/

namespace Mod
{
    public static class StaticValues{
        public static string ModLocation = ModAPI.Metadata.MetaLocation;
    }

    public static class ABloader{
        private static Type _ABType;
        private static MethodInfo _loadFromFile;
        private static MethodInfo _loadFromBundle;
        private static MethodInfo _unloadBundle;
        private static MethodInfo _GetAllLoadedAssetBundles;
        public static T LoadFromAB<T>(object AB, string name)
        {
            return (T)_loadFromBundle.Invoke(AB, new object[] { name, typeof(T) });
        }

        public static object LoadFromFile(string patch)
        {
            object bundle = null;
            foreach (object i in ABloader.GetAllLoadedAssetBundles()){
                if (i.GetPropertyRef<string>("name") == Path.GetFileName(patch)){
                    bundle=i;
                    break;
                }
            }
            if (bundle == null) bundle = _loadFromFile.Invoke(null, new object[] { Path.Combine(StaticValues.ModLocation, patch) });
            return bundle;
        }

        public static void UnloadAB(object AB)
        {
            _unloadBundle.Invoke(AB, new object[] { false });
        }

        public static Assembly DllActivator(string path){
            var Loaderbytes = File.ReadAllBytes(path);
            return Assembly.Load(Loaderbytes);
        }

        public static T GetPropertyRef<T>(this object obj, string nameField)
        {
            return (T)obj.GetType().GetProperty(nameField, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic).GetValue(obj);
        }

        public static object[] GetAllLoadedAssetBundles()=>(object[])_GetAllLoadedAssetBundles.Invoke(null, null);

        static ABloader(){
            _ABType = Type.GetType("UnityEngine.AssetBundle, UnityEngine.AssetBundleModule");

            _loadFromFile = _ABType.GetMethod("LoadFromFile", new[] { typeof(string) });
            _loadFromBundle = _ABType.GetMethod("LoadAsset", new[] { typeof(string), typeof(Type) });
            _unloadBundle = _ABType.GetMethod("Unload");
            _GetAllLoadedAssetBundles = _ABType.GetMethod("GetAllLoadedAssetBundles");
        }
    }

    public class Mod : MonoBehaviour
    {
        //  CATEGORY / MOD NAME
        public static string CategoryName = "Nova's Avengers Mod";


        public static Texture2D DamageTexture = ModAPI.LoadSprite("Art/TempForScripts/DamageTexture.png").texture;

        public static Sprite NoPowerSprite = ModAPI.LoadSprite("Art/UI/Icons/None.png");
        public static Sprite streng = ModAPI.LoadSprite("Art/UI/Icons/Strength.png");
        public static Sprite hea = ModAPI.LoadSprite("Art/UI/Icons/Heal.png");
        public static Sprite Background = ModAPI.LoadSprite("Art/UI/background.png");
        public static Sprite PBackground = ModAPI.LoadSprite("Art/UI/pbackground.png");
        public static Sprite closeSpriteButton = ModAPI.LoadSprite("Art/UI/x.png");
        public static Sprite buttonSprite = ModAPI.LoadSprite("Art/UI/button.png");
        public static Sprite AI = ModAPI.LoadSprite("Art/UI/AI.png");
        public static Sprite Manual = ModAPI.LoadSprite("Art/UI/Manual.png");
        public static Sprite Locked = ModAPI.LoadSprite("Art/UI/Locked.png");
        public static Sprite Unlocked = ModAPI.LoadSprite("Art/UI/Unlocked.png");
        public static Sprite Normal = ModAPI.LoadSprite("Art/UI/Normal.png");
        public static Sprite Connector = ModAPI.LoadSprite("Art/UI/Connector.png");
        public static Sprite Grapple = ModAPI.LoadSprite("Art/UI/Grapple.png");
        public static Sprite Electric = ModAPI.LoadSprite("Art/UI/Electric.png");
        public static Sprite Webshot = ModAPI.LoadSprite("Art/UI/Webshot.png");
        public static Sprite None = ModAPI.LoadSprite("Art/UI/None.png");
        public static Sprite ToggledSprite = ModAPI.LoadSprite("Art/UI/off.png");
        public static Sprite Ice = ModAPI.LoadSprite("Art/Objects/Ice.png");
        public static Sprite IceFull = ModAPI.LoadSprite("Art/Objects/IceFull.png");
        public static Sprite Explosive = ModAPI.LoadSprite("Art/Objects/Explosive.png");
        public static Sprite Pixel = ModAPI.LoadSprite("Art/Objects/RedPixel.png");
        public static Sprite ShockGauntletSprite = ModAPI.LoadSprite("Art/Objects/ShockGauntlet.png");
        public static AudioClip BatarangCharge = ModAPI.LoadSound("Sounds/BatarangCharge.wav");
        public static AudioClip Click = ModAPI.LoadSound("Sounds/Click.wav");
        public static Sprite web = ModAPI.LoadSprite("Art/Objects/Web.png");
        public static Sprite electricWeb = ModAPI.LoadSprite("Art/Objects/ElectricWeb.png");
        public static Sprite WebShot = ModAPI.LoadSprite("Art/Objects/WebShot.png");
        public static Sprite TailEnd = ModAPI.LoadSprite("Art/Skins/Scorpion/TailEnd.png");
        public static Sprite Gripper = ModAPI.LoadSprite("Art/Skins/Doc-Ock/Gripper.png");
        public static Sprite GripperOpen = ModAPI.LoadSprite("Art/Skins/Doc-Ock/GripperOpen.png");
        public static Sprite WebAnchor = ModAPI.LoadSprite("Art/Objects/WebAnchor.png");
        public static AudioClip OctopusWav = ModAPI.LoadSound("Sounds/OctopusWav.wav");
        public static AudioClip[] OctopusStomp = new AudioClip[]{
            ModAPI.LoadSound("Sounds/OctopusStomp1.wav"),
            ModAPI.LoadSound("Sounds/OctopusStomp3.wav")
        };
        public static AudioClip WebStretch = ModAPI.LoadSound("Sounds/WebStretch.wav");
        public static AudioClip[] WebSFX =
        {
            ModAPI.LoadSound("Sounds/Web1.wav"),
            ModAPI.LoadSound("Sounds/Web2.wav"),
            ModAPI.LoadSound("Sounds/Web3.wav"),
            ModAPI.LoadSound("Sounds/Web4.wav"),
            ModAPI.LoadSound("Sounds/Web5.wav"),
            ModAPI.LoadSound("Sounds/Web6.wav")
        };

        public static AudioClip Electricity = ModAPI.LoadSound("Sounds/Electricity.wav");
        public static AudioClip Thunder = ModAPI.LoadSound("Sounds/Thunder.wav");

        public static List<SkinsDictionary> skinsIcons = new List<SkinsDictionary>();

        public struct ColorPlus
        {
            public static Color32 Teal = new Color32(0, 153, 255, 255);
            public static Color32 Brown = new Color32(139, 69, 19, 255);
            public static Color32 Tan = new Color32(210, 180, 140, 255);
            public static Color32 Orange = new Color32(255, 165, 0, 255);
            public static Color32 LightPink = new Color32(255, 182, 193, 255);
            public static Color32 Olive = new Color32(128, 128, 0, 255);
            public static Color32 Navy = new Color32(0, 0, 128, 255);
            public static Color32 Coral = new Color32(255, 127, 80, 255);
            public static Color32 Salmon = new Color32(250, 128, 114, 255);
            public static Color32 Mint = new Color32(189, 252, 201, 255);
            public static Color32 Lavender = new Color32(230, 230, 250, 255);
            public static Color32 Gold = new Color32(255, 215, 0, 255);
        }

        public struct ModAPIPlus
        {
            public interface IUse2
            {
                void Use2();
            }

            public static List<Sprite> LimbSprites(string FilePath)
            {
                List<Sprite> limbSprites = new List<Sprite>();

                string[] spriteNames = { "Head", "UpperBody", "MiddleBody", "LowerBody", "UpperArmFront", "LowerArmFront", "UpperArm", "LowerArm", "UpperLegFront", "LowerLegFront", "FootFront", "UpperLeg", "LowerLeg", "Foot" };

                foreach (string spriteName in spriteNames)
                {
                    try
                    {
                        Debug.Log(FilePath + spriteName + ".png");
                        var sprit = ModAPI.LoadSprite(FilePath + spriteName + ".png");
                        sprit.name = spriteName;
                        limbSprites.Add(sprit);
                    }
                    catch
                    {
                        //Debug.Log("No " + spriteName + " Sprite");
                    }
                }

                return limbSprites;
            }

            public static void AddMoreSkinsToIconChanger(string CharacterName, string icon)
            {
                var realCharacterName = "[" + CategoryName + "] " + CharacterName;
                bool found = true;
                foreach (var skinthing in Mod.skinsIcons)
                {
                    if (skinthing.characterName == realCharacterName)
                    {
                        skinthing.icons.Add(ModAPI.LoadSprite("Art/Thumbnails/" + icon + ".png"));
                        found = false;
                    }
                }

                if (found)
                {
                    var bob = new SkinsDictionary()
                    {
                        characterName = "[" + CategoryName + "] " + CharacterName
                    };
                    bob.icons.Add(ModAPI.LoadSprite("Art/Thumbnails/" + icon + ".png"));
                    Mod.skinsIcons.Add(bob);
                }
            }

            public static ItemButtonBehaviour GetIcon(string spawnableAsset)
            {
                foreach (ItemButtonBehaviour item in GameObject.FindObjectsOfType<ItemButtonBehaviour>(true)) { if (item.Item != null) if (item.Item.name == spawnableAsset) return item; }
                return null;
            }

            public static void CreateHuman(string name, string description, string FileName, string Thumbname, Action<GameObject> AfterSpawn = null, string OrderOverride = null)
            {
                var bob = new SkinsDictionary()
                {
                    characterName = "[" + CategoryName + "] " + name
                };
                bob.icons.Add(ModAPI.LoadSprite("Art/Thumbnails/" + Thumbname + ".png"));
                Mod.skinsIcons.Add(bob);
                ModAPI.Register(
                    new Modification()
                    {
                        OriginalItem = ModAPI.FindSpawnable("Human"),
                        NameOverride = "[" + CategoryName + "] " + name,
                        NameToOrderByOverride = OrderOverride,
                        DescriptionOverride = description,
                        CategoryOverride = ModAPI.FindCategory(CategoryName),
                        ThumbnailOverride = ModAPI.LoadSprite("Art/Thumbnails/" + Thumbname + ".png"),
                        AfterSpawn = (Instance) =>
                        {
                            var person = Instance.GetComponent<PersonBehaviour>();
                            var skin = ModAPI.LoadTexture("Art/Skins/" + FileName + "/Skin.png");
                            person.SetBodyTextures(skin);

                            var menu = Instance.AddComponent<TextureMenu>();
                            menu.CreateUI();
                            menu.AddButton("Default", ModAPI.LoadSprite("Art/Thumbnails/" + Thumbname + ".png"), ModAPIPlus.LimbSprites("Art/Skins/" + FileName + "/"), null, null, ModAPI.LoadSprite("Art/Skins/" + FileName + "/Cape.png"), ModAPI.LoadSprite("Art/Skins/" + FileName + "/CapeThing.png"));

                            //body code thing
                            Timtam.MakeCustomSkin(person, ModAPIPlus.LimbSprites("Art/Skins/" + FileName + "/"), false, true);
                        }
                        + AfterSpawn
                        + new Action<GameObject>((Instance) =>
                        {
                            var menu = Instance.GetComponent<TextureMenu>();
                            var spawnIconButton = ModAPIPlus.GetIcon("[" + CategoryName + "] " + name);
                            foreach (var button in menu.Buttons)
                            {
                                if (button.GetComponent<Image>().sprite == spawnIconButton.GetComponent<Image>().sprite)
                                {
                                    button.GetComponent<Button>().onClick.Invoke();
                                    menu.ChangeTexture(menu.SelectedSkin);
                                }
                            }
                        })
                    }
                );
            }

            public static void CreateObject(string originalObject, string name, string description, string FileName, string Thumbname, Action<GameObject> AfterSpawn = null, string OrderOverride = null, bool ChangeColsAndMass = true)
            {
                ModAPI.Register(
                    new Modification()
                    {
                        OriginalItem = ModAPI.FindSpawnable(originalObject),
                        NameOverride = "["+ CategoryName +"] " + name,
                        NameToOrderByOverride = OrderOverride,
                        DescriptionOverride = description,
                        CategoryOverride = ModAPI.FindCategory(CategoryName),
                        ThumbnailOverride = ModAPI.LoadSprite("Art/Thumbnails/" + Thumbname + ".png"),
                        AfterSpawn = (Instance) =>
                        {
                            if (Instance.GetComponent<RandomSpriteBehaviour>())
                            {
                                Destroy(Instance.GetComponent<RandomSpriteBehaviour>());
                            }

                            Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("Art/Objects/" + FileName + ".png");
                            Instance.GetComponent<PhysicalBehaviour>().RefreshOutline();
                            if (ChangeColsAndMass)
                            {
                                Instance.FixColliders();
                                Instance.GetComponent<PhysicalBehaviour>().RecalculateMassBasedOnSize();
                            }

                        }
                        + AfterSpawn
                    }
                );
            }

        }

        public static void OnLoad()
        {
            ModAPI.RegisterInput("Nova's Custom Input Key", "NovaKeyActivation", KeyCode.H);

            MrNegative.DevilEffect = ABloader.LoadFromAB<GameObject>(ABloader.LoadFromFile("AssetBundles/devileffect"), "Devil Effect");
            MrNegative.DevilAura = ABloader.LoadFromAB<GameObject>(ABloader.LoadFromFile("AssetBundles/devileffect"), "Devil Aura");
            MrNegative.EnergyBlast = ABloader.LoadFromAB<GameObject>(ABloader.LoadFromFile("AssetBundles/devileffect"), "Energy Blast");
            MrNegative.DevilExplosion = ABloader.LoadFromAB<GameObject>(ABloader.LoadFromFile("AssetBundles/devileffect"), "Devil Explosion");
            MrNegative.EnergyCharge = ABloader.LoadFromAB<GameObject>(ABloader.LoadFromFile("AssetBundles/devileffect"), "Energy Charge");
            TextureMenu.MenuPrefab = ABloader.LoadFromAB<GameObject>(ABloader.LoadFromFile("AssetBundles/menus"), "Skin Menu");
            InternalPowerMenu.MenuPrefab = ABloader.LoadFromAB<GameObject>(ABloader.LoadFromFile("AssetBundles/menus"), "InternalAbilityMenu");
            PowerMenu.MenuPrefab = ABloader.LoadFromAB<GameObject>(ABloader.LoadFromFile("AssetBundles/menus"), "Ability Menu");
            AbilityMenus.MenuPrefab = ABloader.LoadFromAB<GameObject>(ABloader.LoadFromFile("AssetBundles/menus"), "Ability Menus");
            TextureMenu.MenuPrefab = ABloader.LoadFromAB<GameObject>(ABloader.LoadFromFile("AssetBundles/menus"), "Skin Menu");

            MrNegative.DarknessPrefab=ModAPI.FindSpawnable("Decimator").Prefab.GetComponent<DecimatorBehaviour>().BlackHole.transform.Find("darkness").gameObject;
        }

        public static void Main()
        {
            //Category
             ModAPI.RegisterCategory(CategoryName, "Category for Nova's Avengers Mod", ModAPI.LoadSprite("icon.png"));

             //Tony Stark
            ModAPIPlus.CreateHuman("Tony Stark", "", "Tony Stark", "Tony Stark", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();
      
                var menu = Instance.GetComponent<TextureMenu>();
                menu.AddButton("Iron Man", ModAPI.LoadSprite("Art/Thumbnails/Iron Man.png"), ModAPIPlus.LimbSprites("Art/Skins/Iron Man/"));
                menu.AddButton("Unmasked", ModAPI.LoadSprite("Art/Thumbnails/Iron Man Unmasked.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Iron Man Unmasked/"));

            }, "a");

            //Steve Rogers
            ModAPIPlus.CreateHuman("Steve Rogers", "", "Steve Rogers", "Steve Rogers", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();
                SpeedHealing.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Heal.png"));
                SuperMass.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Strength.png"));

                person.GetComponent<SpeedHealing>().EnablePower();
                person.GetComponent<SuperMass>().EnablePower();

                var menu = Instance.GetComponent<TextureMenu>();
                menu.AddButton("Unmasked", ModAPI.LoadSprite("Art/Thumbnails/Steve Rodgers Unmasked.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Steve Rodgers Unmasked/"));

            }, "a");

            //Sam Wilson
            ModAPIPlus.CreateHuman("Sam Wilson", "", "Sam Wilson", "Sam Wilson", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();              
                var menu = Instance.GetComponent<TextureMenu>();

            }, "a");

            //Thor
            ModAPIPlus.CreateHuman("Thor", "", "Thor", "Thor", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();
                SpeedHealing.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Heal.png"));
                SuperMass.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Strength.png"));

                person.GetComponent<SpeedHealing>().EnablePower();
                person.GetComponent<SuperMass>().EnablePower();


                foreach (var Limbs in Instance.GetComponent<PersonBehaviour>().Limbs)
                {
                    if (Limbs.gameObject.name.Contains("ArmFront"))
                    {
                        Limbs.GetComponent<SpriteRenderer>().sortingLayerName = "Top";
                    }
                    if (Limbs.gameObject.name.Contains("ArmFront"))
                    {
                        Limbs.GetComponent<SpriteRenderer>().sortingLayerName = "Top";
                    }

                    if (Limbs.name.Contains("UpperBody"))
                    {
                        Limbs.GetComponent<SpriteRenderer>().sortingLayerName = "Top";
                    }

                    if (Limbs.gameObject.name.Contains("LegFront") || Limbs.gameObject.name.Contains("FootFront"))
                    {
                        Limbs.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
                    }

                    if (Limbs.name.Contains("LowerBody"))
                    {
                        Limbs.GetComponent<SpriteRenderer>().sortingOrder += 4;
                    }
                }

                Cape.CreateCapeForPerson(person, ModAPI.LoadSprite("Art/Skins/Thor/Cape.png").texture, ModAPI.LoadSprite("Art/Skins/Thor/CapeThing.png"));

            }, "a");

            //Bruce Banner
            ModAPIPlus.CreateHuman("Bruce Banner", "", "Bruce Banner", "Bruce Banner", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();
                
                var menu = Instance.GetComponent<TextureMenu>();

                menu.AddButton("First Apearance", ModAPI.LoadSprite("Art/Thumbnails/First Apearance Bruce Banner.png"), ModAPIPlus.LimbSprites("Art/AltSkins/First Apearance Bruce Banner/"));

            }, "a");

            //Hulk
            ModAPIPlus.CreateHuman("Hulk", "", "Hulk", "Hulk", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();

                var menu = Instance.GetComponent<TextureMenu>();

                menu.AddButton("Maestro", ModAPI.LoadSprite("Art/Thumbnails/Maestro.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Maestro/"));
                menu.AddButton("First Apearance", ModAPI.LoadSprite("Art/Thumbnails/First Apearance Hulk.png"), ModAPIPlus.LimbSprites("Art/AltSkins/First Apearance Hulk/"));
                menu.AddButton("Gray", ModAPI.LoadSprite("Art/Thumbnails/Gray Hulk.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Gray Hulk/"));
                menu.AddButton("Gladiator", ModAPI.LoadSprite("Art/Thumbnails/Gladiator.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Gladiator/"));
                menu.AddButton("Professor", ModAPI.LoadSprite("Art/Thumbnails/Professor Hulk.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Professor Hulk/"));
                menu.AddButton("Smart", ModAPI.LoadSprite("Art/Thumbnails/Smart Hulk.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Smart Hulk/"));
                if (Instance.transform.localScale.x > 0)
                {
                    Instance.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
                }
                else
                {
                    Instance.transform.localScale = new Vector3(-1.25f, 1.25f, 1.25f);
                }
            }, "a");

            //Jennifer Walters
            ModAPIPlus.CreateHuman("Jennifer Walters", "Justice doesn't flinch. Neither do I.", "Jennifer Walters", "Jennifer Walters", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();

                var menu = Instance.GetComponent<TextureMenu>();

            }, "a");

            //She-Hulk
            ModAPIPlus.CreateHuman("She-Hulk", "Justice doesn't flinch. Neither do I.", "She-Hulk", "She-Hulk", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();

                var menu = Instance.GetComponent<TextureMenu>();
                menu.AddButton("F4", ModAPI.LoadSprite("Art/Thumbnails/She-Hulk F4.png"), ModAPIPlus.LimbSprites("Art/AltSkins/She-Hulk F4/"));
            }, "a");

            //Black Widow
            ModAPIPlus.CreateHuman("Black Widow", "", "Black Widow", "Black Widow", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();

                var menu = Instance.GetComponent<TextureMenu>();
               
            }, "a");

            //Yelena Belova
            ModAPIPlus.CreateHuman("Yelena Belova", "Cute plan. Mind if I ruin it?", "Yelena Belova", "Yelena Belova", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();

                var menu = Instance.GetComponent<TextureMenu>();

                menu.AddButton("Black Widow", ModAPI.LoadSprite("Art/Thumbnails/Black Widow Yelena.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Black Widow Yelena/"));
                menu.AddButton("Thunderbolts", ModAPI.LoadSprite("Art/Thumbnails/Thunderbolts Yelena.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Thunderbolts Yelena/"));
               
            }, "a");

            //Hawkeye
            ModAPIPlus.CreateHuman("Hawkeye", "", "Hawkeye", "Hawkeye", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();

                var menu = Instance.GetComponent<TextureMenu>();
                menu.AddButton("Clint Barton", ModAPI.LoadSprite("Art/Thumbnails/Clint Barton.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Clint Barton/"));
                menu.AddButton("Classic Suit", ModAPI.LoadSprite("Art/Thumbnails/Classic Hawkeye.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Classic Hawkeye/"));
            }, "a");

            //Wanda
            ModAPIPlus.CreateHuman("Wanda Maximoff", "", "Wanda", "Wanda", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();

                var menu = Instance.GetComponent<TextureMenu>();
                menu.AddButton("Casual", ModAPI.LoadSprite("Art/Thumbnails/Wanda Casual.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Wanda Casual/"));
                menu.AddButton("Hellfire Gala", ModAPI.LoadSprite("Art/Thumbnails/Hellfire Gala.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Hellfire Gala/"));
                menu.AddButton("Civil War", ModAPI.LoadSprite("Art/Thumbnails/Wanda Civil War.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Wanda Civil War/"));
                menu.AddButton("Multiverse of Madness", ModAPI.LoadSprite("Art/Thumbnails/MOM Wanda.png"), ModAPIPlus.LimbSprites("Art/AltSkins/MOM Wanda/"));
            }, "a");

            //Quicksilver
            ModAPIPlus.CreateHuman("Quicksilver", "", "Quicksilver", "Quicksilver", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();
                SpeedHealing.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Heal.png"));
                SuperMass.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Strength.png"));

                person.GetComponent<SpeedHealing>().EnablePower();
                person.GetComponent<SuperMass>().EnablePower();

                var menu = Instance.GetComponent<TextureMenu>();
                menu.AddButton("Green", ModAPI.LoadSprite("Art/Thumbnails/Quicksilver Green.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Quicksilver Green/"));
                menu.AddButton("Casual", ModAPI.LoadSprite("Art/Thumbnails/Quicksilver Casual.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Quicksilver Casual/"));
                menu.AddButton("Mcu", ModAPI.LoadSprite("Art/Thumbnails/Quicksilver Age Of Ultron.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Quicksilver Age Of Ultron/"));
                menu.AddButton("Peters", ModAPI.LoadSprite("Art/Thumbnails/Quicksilver Evan Peters.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Quicksilver Evan Peters/"));

            }, "a");

            //Vision
            ModAPIPlus.CreateHuman("Vision", "", "Vision", "Vision", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();
                SpeedHealing.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Heal.png"));
                SuperMass.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Strength.png"));

                person.GetComponent<SpeedHealing>().EnablePower();
                person.GetComponent<SuperMass>().EnablePower();


                foreach (var Limbs in Instance.GetComponent<PersonBehaviour>().Limbs)
                {
                    if (Limbs.gameObject.name.Contains("ArmFront"))
                    {
                        Limbs.GetComponent<SpriteRenderer>().sortingLayerName = "Top";
                    }
                    if (Limbs.gameObject.name.Contains("ArmFront"))
                    {
                        Limbs.GetComponent<SpriteRenderer>().sortingLayerName = "Top";
                    }

                    if (Limbs.name.Contains("UpperBody"))
                    {
                        Limbs.GetComponent<SpriteRenderer>().sortingLayerName = "Top";
                    }

                    if (Limbs.gameObject.name.Contains("LegFront") || Limbs.gameObject.name.Contains("FootFront"))
                    {
                        Limbs.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
                    }

                    if (Limbs.name.Contains("LowerBody"))
                    {
                        Limbs.GetComponent<SpriteRenderer>().sortingOrder += 4;
                    }
                }

                Cape.CreateCapeForPerson(person, ModAPI.LoadSprite("Art/Skins/Vision/Cape.png").texture, ModAPI.LoadSprite("Art/Skins/Vision/CapeThing.png"));

            }, "a");

            //Antman
            ModAPIPlus.CreateHuman("Antman", "", "Antman", "Antman", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();

                var menu = Instance.GetComponent<TextureMenu>();

                menu.AddButton("Unmasked", ModAPI.LoadSprite("Art/Thumbnails/Antman Unmasked.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Antman Unmasked/"));
                menu.AddButton("MCU", ModAPI.LoadSprite("Art/Thumbnails/Antman Civil War.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Antman Civil War/"));
                menu.AddButton("Giant-Man", ModAPI.LoadSprite("Art/Thumbnails/Giant-Man.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Giant-Man/"));
            }, "a");


            //Doctor Strange
            ModAPIPlus.CreateHuman("Doctor Strange", "", "Doctor Strange", "Doctor Strange", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();
                SpeedHealing.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Heal.png"));
                SuperMass.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Strength.png"));

                person.GetComponent<SpeedHealing>().EnablePower();
                person.GetComponent<SuperMass>().EnablePower();


                foreach (var Limbs in Instance.GetComponent<PersonBehaviour>().Limbs)
                {
                    if (Limbs.gameObject.name.Contains("ArmFront"))
                    {
                        Limbs.GetComponent<SpriteRenderer>().sortingLayerName = "Top";
                    }
                    if (Limbs.gameObject.name.Contains("ArmFront"))
                    {
                        Limbs.GetComponent<SpriteRenderer>().sortingLayerName = "Top";
                    }

                    if (Limbs.name.Contains("UpperBody"))
                    {
                        Limbs.GetComponent<SpriteRenderer>().sortingLayerName = "Top";
                    }

                    if (Limbs.gameObject.name.Contains("LegFront") || Limbs.gameObject.name.Contains("FootFront"))
                    {
                        Limbs.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
                    }

                    if (Limbs.name.Contains("LowerBody"))
                    {
                        Limbs.GetComponent<SpriteRenderer>().sortingOrder += 4;
                    }
                }

                Cape.CreateCapeForPerson(person, ModAPI.LoadSprite("Art/Skins/Doctor Strange/Cape.png").texture, ModAPI.LoadSprite("Art/Skins/Doctor Strange/CapeThing.png"));

            }, "a");

            //Black Panther
            ModAPIPlus.CreateHuman("Black Panther", "", "Black Panther", "Black Panther", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();
                SpeedHealing.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Heal.png"));
                SuperMass.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Strength.png"));

                person.GetComponent<SpeedHealing>().EnablePower();
                person.GetComponent<SuperMass>().EnablePower();


                foreach (var Limbs in Instance.GetComponent<PersonBehaviour>().Limbs)
                {
                    if (Limbs.gameObject.name.Contains("ArmFront"))
                    {
                        Limbs.GetComponent<SpriteRenderer>().sortingLayerName = "Top";
                    }
                    if (Limbs.gameObject.name.Contains("ArmFront"))
                    {
                        Limbs.GetComponent<SpriteRenderer>().sortingLayerName = "Top";
                    }

                    if (Limbs.name.Contains("UpperBody"))
                    {
                        Limbs.GetComponent<SpriteRenderer>().sortingLayerName = "Top";
                    }

                    if (Limbs.gameObject.name.Contains("LegFront") || Limbs.gameObject.name.Contains("FootFront"))
                    {
                        Limbs.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
                    }

                    if (Limbs.name.Contains("LowerBody"))
                    {
                        Limbs.GetComponent<SpriteRenderer>().sortingOrder += 4;
                    }
                }

                Cape.CreateCapeForPerson(person, ModAPI.LoadSprite("Art/Skins/Black Panther/Cape.png").texture, ModAPI.LoadSprite("Art/Skins/Black Panther/CapeThing.png"));

            }, "a");

            //Blue Marvel
            ModAPIPlus.CreateHuman("Blue Marvel", "", "Blue Marvel", "Blue Marvel", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();
                SpeedHealing.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Heal.png"));
                SuperMass.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Strength.png"));

                person.GetComponent<SpeedHealing>().EnablePower();
                person.GetComponent<SuperMass>().EnablePower();

                var menu = Instance.GetComponent<TextureMenu>();

                menu.AddButton("Jacket", ModAPI.LoadSprite("Art/Thumbnails/Blue Marvel Jacket.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Blue Marvel Jacket/"));


                foreach (var Limbs in Instance.GetComponent<PersonBehaviour>().Limbs)
                {
                    if (Limbs.gameObject.name.Contains("ArmFront"))
                    {
                        Limbs.GetComponent<SpriteRenderer>().sortingLayerName = "Top";
                    }
                    if (Limbs.gameObject.name.Contains("ArmFront"))
                    {
                        Limbs.GetComponent<SpriteRenderer>().sortingLayerName = "Top";
                    }

                    if (Limbs.name.Contains("UpperBody"))
                    {
                        Limbs.GetComponent<SpriteRenderer>().sortingLayerName = "Top";
                    }

                    if (Limbs.gameObject.name.Contains("LegFront") || Limbs.gameObject.name.Contains("FootFront"))
                    {
                        Limbs.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
                    }

                    if (Limbs.name.Contains("LowerBody"))
                    {
                        Limbs.GetComponent<SpriteRenderer>().sortingOrder += 4;
                    }
                }

                Cape.CreateCapeForPerson(person, ModAPI.LoadSprite("Art/Skins/Blue Marvel/Cape.png").texture, ModAPI.LoadSprite("Art/Skins/Blue Marvel/CapeThing.png"));

            }, "a");

            //Iron Spider
            ModAPIPlus.CreateHuman("Iron Spider", "With Great Power... Comes Great Responsibility. <color=\"yellow\">\n \n- Activate arm to shoot webs \n- Activate with custom activator (typically H) to change web type (Normal, Connector, Grapple, Electric, Webshot, None)", "Iron Spider", "Iron Spider", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();
                SpeedHealing.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Heal.png"));
                SuperMass.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Strength.png"));
                Fighter.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Fight.png"));

                person.GetComponent<SpeedHealing>().EnablePower();
                person.GetComponent<SuperMass>().EnablePower();
                person.GetComponent<Fighter>().EnablePower();

                var menu = Instance.GetComponent<TextureMenu>();

                UnityEvent SkinLayerAddEvent = new UnityEvent();

                SkinLayerAddEvent.AddListener(new UnityAction(() =>
                {
                    foreach (var Limbs in Instance.GetComponent<PersonBehaviour>().Limbs)
                    {
                        if (Limbs.gameObject.name.Contains("ArmFront"))
                        {
                            Limbs.GetComponent<SpriteRenderer>().sortingLayerName = "Top";
                        }

                        if (Limbs.gameObject.name.Contains("LegFront") || Limbs.gameObject.name.Contains("FootFront"))
                        {
                            Limbs.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
                        }

                        if (Limbs.name.Contains("LowerBody"))
                        {
                            Limbs.GetComponent<SpriteRenderer>().sortingOrder += 4;
                        }
                    }
                }));

                UnityEvent SkinLayerRemoveEvent = new UnityEvent();

                SkinLayerRemoveEvent.AddListener(new UnityAction(() =>
                {
                    foreach (var Limbs in Instance.GetComponent<PersonBehaviour>().Limbs)
                    {
                        if (Limbs.gameObject.name.Contains("ArmFront"))
                        {
                            Limbs.GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";
                        }

                        if (Limbs.gameObject.name.Contains("LegFront") || Limbs.gameObject.name.Contains("FootFront"))
                        {
                            Limbs.GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";
                        }

                        if (Limbs.name.Contains("LowerBody"))
                        {
                            Limbs.GetComponent<SpriteRenderer>().sortingOrder -= 4;
                        }
                    }
                }));

                menu.AddButton("Unmasked", ModAPI.LoadSprite("Art/Thumbnails/Iron Spider Unmasked.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Iron Spider Unmasked/"));

                foreach (var limb in person.Limbs)
                {
                    if (limb.gameObject.name.Contains("LowerArm"))
                    {
                        WebSlinging.SetPower(person, limb, ModAPI.LoadSprite("Art/UI/Icons/Web.png"));
                        limb.GetComponent<WebSlinging>().EnablePower();
                    }
                }

            }, "a");

            //Nick Fury Sr
            ModAPIPlus.CreateHuman("Nick Fury Sr", "", "Nick Fury Sr", "Nick Fury Sr", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();
                SpeedHealing.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Heal.png"));
                SuperMass.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Strength.png"));

                person.GetComponent<SpeedHealing>().EnablePower();
                person.GetComponent<SuperMass>().EnablePower();

                var menu = Instance.GetComponent<TextureMenu>();
                menu.AddButton("Eyepatch", ModAPI.LoadSprite("Art/Thumbnails/Nick Fury Sr Eyepatch.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Nick Fury Sr Eyepatch/"));
                menu.AddButton("Nick Fury Jr", ModAPI.LoadSprite("Art/Thumbnails/Nick Fury Shield.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Nick Fury Shield/"));
                menu.AddButton("Nick Fury Jr Eyepatch", ModAPI.LoadSprite("Art/Thumbnails/Nick Fury Shield Eyepatch.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Nick Fury Shield Eyepatch/"));

            }, "a");

            //Binary
            ModAPIPlus.CreateHuman("Binary", "I don't burn out. I go supernova.", "Binary", "Binary", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();

                var menu = Instance.GetComponent<TextureMenu>();
               
            }, "a");

            //Maria Hill
            ModAPIPlus.CreateHuman("Maria Hill", "I don't do warnings. I do results.", "Maria Hill", "Maria Hill", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();

                var menu = Instance.GetComponent<TextureMenu>();

                menu.AddButton("Casual", ModAPI.LoadSprite("Art/Thumbnails/Casual Maria Hill.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Casual Maria Hill/"));
               
            }, "a");

            //Wiccan
            ModAPIPlus.CreateHuman("Wiccan", "Magic's not just power. It's purpose.", "Wiccan", "Wiccan", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();
                SpeedHealing.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Heal.png"));
                SuperMass.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Strength.png"));

                person.GetComponent<SpeedHealing>().EnablePower();
                person.GetComponent<SuperMass>().EnablePower();

                var menu = Instance.GetComponent<TextureMenu>();

                menu.AddButton("Casual", ModAPI.LoadSprite("Art/Thumbnails/Casual Wiccan.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Casual Wiccan/"));


                foreach (var Limbs in Instance.GetComponent<PersonBehaviour>().Limbs)
                {
                    if (Limbs.gameObject.name.Contains("ArmFront"))
                    {
                        Limbs.GetComponent<SpriteRenderer>().sortingLayerName = "Top";
                    }
                    if (Limbs.gameObject.name.Contains("ArmFront"))
                    {
                        Limbs.GetComponent<SpriteRenderer>().sortingLayerName = "Top";
                    }

                    if (Limbs.name.Contains("UpperBody"))
                    {
                        Limbs.GetComponent<SpriteRenderer>().sortingLayerName = "Top";
                    }

                    if (Limbs.gameObject.name.Contains("LegFront") || Limbs.gameObject.name.Contains("FootFront"))
                    {
                        Limbs.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
                    }

                    if (Limbs.name.Contains("LowerBody"))
                    {
                        Limbs.GetComponent<SpriteRenderer>().sortingOrder += 4;
                    }
                }

                Cape.CreateCapeForPerson(person, ModAPI.LoadSprite("Art/Skins/Wiccan/Cape.png").texture, ModAPI.LoadSprite("Art/Skins/Wiccan/CapeThing.png"));

            }, "a");

            //Speed
            ModAPIPlus.CreateHuman("Speed", "Catch me if you can. Spoiler: you can't.", "Speed", "Speed", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();
                SpeedHealing.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Heal.png"));
                SuperMass.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Strength.png"));

                person.GetComponent<SpeedHealing>().EnablePower();
                person.GetComponent<SuperMass>().EnablePower();

                var menu = Instance.GetComponent<TextureMenu>();

                menu.AddButton("Casual", ModAPI.LoadSprite("Art/Thumbnails/Casual Speed.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Casual Speed/"));

            }, "a");

            //Hulkling
            ModAPIPlus.CreateHuman("Hulkling", "Peace takes strength. I've got both.", "Hulkling", "Hulkling", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();
                SpeedHealing.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Heal.png"));
                SuperMass.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Strength.png"));

                person.GetComponent<SpeedHealing>().EnablePower();
                person.GetComponent<SuperMass>().EnablePower();

                var menu = Instance.GetComponent<TextureMenu>();

                menu.AddButton("Casual", ModAPI.LoadSprite("Art/Thumbnails/Casual Hulkling.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Casual Hulkling/"));

                if (Instance.transform.localScale.x > 0)
                {
                    Instance.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                }
                else
                {
                    Instance.transform.localScale = new Vector3(-1.2f, 1.2f, 1.2f);
                }
            }, "a");

            //Patriot
            ModAPIPlus.CreateHuman("Patriot", "Legacy isn't given. It's earned one fight at a time.", "Patriot", "Patriot", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();
                SpeedHealing.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Heal.png"));
                SuperMass.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Strength.png"));

                person.GetComponent<SpeedHealing>().EnablePower();
                person.GetComponent<SuperMass>().EnablePower();

                var menu = Instance.GetComponent<TextureMenu>();

                menu.AddButton("Fully Masked", ModAPI.LoadSprite("Art/Thumbnails/Patriot Fully Masked.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Patriot Fully Masked/"));
                menu.AddButton("Casual", ModAPI.LoadSprite("Art/Thumbnails/Casual Patriot.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Casual Patriot/"));

            }, "a");

            //Stature
            ModAPIPlus.CreateHuman("Stature", "Big problems? I've got a bigger solution.", "Stature", "Stature", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();

                var menu = Instance.GetComponent<TextureMenu>();
                menu.AddButton("Stinger", ModAPI.LoadSprite("Art/Thumbnails/Stinger.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Stinger/"));
                menu.AddButton("Mcu", ModAPI.LoadSprite("Art/Thumbnails/Stinger Mcu.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Stinger Mcu/"));
                menu.AddButton("Casual", ModAPI.LoadSprite("Art/Thumbnails/Casual Stature.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Casual Stature/"));
               
            }, "a");

            //America Chavez
            ModAPIPlus.CreateHuman("America Chavez", "I punch star-shaped holes in reality. What's your superpower?", "America Chavez", "America Chavez", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();
                SpeedHealing.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Heal.png"));
                SuperMass.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Strength.png"));

                person.GetComponent<SpeedHealing>().EnablePower();
                person.GetComponent<SuperMass>().EnablePower();

                var menu = Instance.GetComponent<TextureMenu>();
               
            }, "a");

            //Skarr
            ModAPIPlus.CreateHuman("Skarr", "I'm not my father. I'm worse.", "Skarr", "Skarr", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();

                var menu = Instance.GetComponent<TextureMenu>();
                if (Instance.transform.localScale.x > 0)
                {
                    Instance.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
                }
                else
                {
                    Instance.transform.localScale = new Vector3(-1.25f, 1.25f, 1.25f);
                }
            }, "a");

            //Loki
            ModAPIPlus.CreateHuman("Loki Laufeyson", "", "Loki", "Loki", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();

                foreach (var Limbs in Instance.GetComponent<PersonBehaviour>().Limbs)
                {
                    if (Limbs.gameObject.name.Contains("ArmFront"))
                    {
                        Limbs.GetComponent<SpriteRenderer>().sortingLayerName = "Top";
                    }
                    if (Limbs.name.Contains("UpperBody"))
                    {
                        Limbs.GetComponent<SpriteRenderer>().sortingLayerName = "Top";
                    }

                    if (Limbs.gameObject.name.Contains("LegFront") || Limbs.gameObject.name.Contains("FootFront"))
                    {
                        Limbs.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
                    }

                    if (Limbs.name.Contains("LowerBody"))
                    {
                        Limbs.GetComponent<SpriteRenderer>().sortingOrder += 4;
                    }
                }
                var menu = Instance.GetComponent<TextureMenu>();
                menu.AddButton("End of Time", ModAPI.LoadSprite("Art/Thumbnails/Loki End of Time.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Loki End of Time/"));
            }, "a");

            //Thanos
            ModAPIPlus.CreateHuman("Thanos", "", "Thanos", "Thanos", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();

                var menu = Instance.GetComponent<TextureMenu>();
                menu.AddButton("Infinity War", ModAPI.LoadSprite("Art/Thumbnails/Thanos Infinity War.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Thanos Infinity War/"));

                foreach (var Limbs in Instance.GetComponent<PersonBehaviour>().Limbs)
                {
                    if (Limbs.gameObject.name.Contains("ArmFront"))
                    {
                        Limbs.GetComponent<SpriteRenderer>().sortingLayerName = "Top";
                    }
                    if (Limbs.gameObject.name.Contains("ArmFront"))
                    {
                        Limbs.GetComponent<SpriteRenderer>().sortingLayerName = "Top";
                    }

                    if (Limbs.name.Contains("UpperBody"))
                    {
                        Limbs.GetComponent<SpriteRenderer>().sortingLayerName = "Top";
                    }

                    if (Limbs.gameObject.name.Contains("LegFront") || Limbs.gameObject.name.Contains("FootFront"))
                    {
                        Limbs.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
                    }

                    if (Limbs.name.Contains("LowerBody"))
                    {
                        Limbs.GetComponent<SpriteRenderer>().sortingOrder += 4;
                    }
                }
                foreach (var limb in person.Limbs)
                {
                    limb.gameObject.FixColliders();
                }

                if (Instance.transform.localScale.x > 0)
                {
                    Instance.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
                }
                else
                {
                    Instance.transform.localScale = new Vector3(-1.25f, 1.25f, 1.25f);
                }


                foreach (var limb in person.Limbs)
                {
                    foreach (var limb2 in person.Limbs)
                    {
                        Physics2D.IgnoreCollision(limb.GetComponent<Collider2D>(), limb2.GetComponent<Collider2D>(), true);
                        limb2.gameObject.AddComponent<NoCollidea>().other = limb.gameObject;
                    }
                }

                
            }, "a");

            //Chitauri
            ModAPIPlus.CreateHuman("Chitauri", "", "Chitauri", "Chitauri", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();

                var menu = Instance.GetComponent<TextureMenu>();

                foreach (var Limbs in Instance.GetComponent<PersonBehaviour>().Limbs)
                {
                    if (Limbs.gameObject.name.Contains("ArmFront"))
                    {
                        Limbs.GetComponent<SpriteRenderer>().sortingLayerName = "Top";
                    }
                    if (Limbs.gameObject.name.Contains("ArmFront"))
                    {
                        Limbs.GetComponent<SpriteRenderer>().sortingLayerName = "Top";
                    }

                    if (Limbs.name.Contains("UpperBody"))
                    {
                        Limbs.GetComponent<SpriteRenderer>().sortingLayerName = "Top";
                    }

                    if (Limbs.gameObject.name.Contains("LegFront") || Limbs.gameObject.name.Contains("FootFront"))
                    {
                        Limbs.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
                    }

                    if (Limbs.name.Contains("LowerBody"))
                    {
                        Limbs.GetComponent<SpriteRenderer>().sortingOrder += 4;
                    }
                }
            }, "a");

            //Objects

            //Tesseract
            ModAPIPlus.CreateObject("Rod", "Tesseract", "", "Tesseract", "Tesseract", (Instance) =>
            {
            }, "2");

            //Cap's Sheild
            ModAPIPlus.CreateObject("Rod", "Cap's Sheild", "", "Cap's Sheild", "Cap's Sheild", (Instance) =>
            {
            }, "2");

            //Mjolnir
            ModAPIPlus.CreateObject("Rod", "Mjolnir", "", "Mjolnir", "Mjolnir", (Instance) =>
            {
            }, "2");

            ModAPI.Register<AlternateMouseActivator>();
            ModAPI.Register<CategoryButtonEditor>();
        }
    }

    public class Phase : Power, Messages.IUse
    {
        public bool Vibrating = false;
        public PersonBehaviour person;

        public static Power SetPower(PersonBehaviour Person, LimbBehaviour TargetLimb, Sprite icon)
        {
            var power = TargetLimb.gameObject.AddComponent<Phase>();
            power.person = Person;
            power.Name = "Invisibility";
            power.Description = "Allows the user to become invisible\n<color=\"yellow\">Disabling the power will revert the user back to their original form, but they can also be reverted by activating the head again";
            power.icon = icon;
            power.targetLimb = TargettedLimb.Head;
            return power;
        }

        public void Use(ActivationPropagation activation)
        {
            if (Enabled == false)
            {
                return;
            }

            Vibrating = !Vibrating;
            foreach (var limb in person.Limbs)
            {
                limb.GetComponent<SpriteRenderer>().color = Vibrating ? new Color(1, 1, 1, 0.1f) : Color.white;
            }
        }
    }

    public class Transformation : Power, Messages.IUse
    {
        bool transformed = false;
        bool transforming = false;
        public PersonBehaviour LastHit;
        public List<Sprite> OGSkin;

        public static Power SetPower(PersonBehaviour Person, LimbBehaviour Limb, Sprite icon)
        {
            var power = Limb.gameObject.AddComponent<Transformation>();
            power.Name = "Transformation";
            power.Description = "Allows the user to transform into the last person they last touched\n<color=\"yellow\">Disabling the power will revert the user back to their original form, but they can also be reverted by activating the head again.";
            power.icon = icon;
            power.targetLimb = TargettedLimb.Head;

            foreach (var limb in Person.Limbs)
            {
                limb.gameObject.AddComponent<ILikeToTouchPeople>();
            }

            return power;
        }

        public void Use(ActivationPropagation activation)
        {
            foreach (var limb in GetComponent<LimbBehaviour>().Person.Limbs)
                if (limb.GetComponent<SpriteMergerAnimator>())
                    return;
            if (transforming || !Enabled)
                return;

            if (GetComponent<LimbBehaviour>().Person.IsAlive() && LastHit != null && transformed != true)
            {
                OGSkin = new List<Sprite>();
                foreach (var limb in transform.root.GetComponent<PersonBehaviour>().Limbs)
                    OGSkin.Add(Instantiate(limb.GetComponent<SpriteRenderer>().sprite));
                transformed = true;
                if (LastHit != null)
                {
                    foreach (var lim in LastHit.Limbs)
                    {
                        foreach (var limb in GetComponent<LimbBehaviour>().Person.Limbs)
                        {
                            if (limb.name == lim.name && !limb.GetComponent<SpriteMergerAnimator>())
                            {
                                limb.gameObject.AddComponent<SpriteMergerAnimator>().Initialize(limb.gameObject.GetComponent<SpriteRenderer>().sprite, lim.SkinMaterialHandler.renderer.sprite, limb.gameObject.GetComponent<SpriteRenderer>().material, true);
                                if (lim.gameObject.TryGetComponent<Cape>(out var cape))
                                {
                                    Cape.CreateCapeForPerson(limb.Person, cape.capeTexture, cape.CapeCollar);
                                }
                            }
                        }
                    }
                }
            }
            else if (GetComponent<LimbBehaviour>().Person.IsAlive() && transformed == true)
            {
                transformed = false;
                transforming = true;
                Timtam.MakeCustomSkinAnimated(GetComponent<LimbBehaviour>().Person, OGSkin, false, true);
                foreach (var limb in GetComponent<LimbBehaviour>().Person.Limbs)
                {
                    if (limb.TryGetComponent<Cape>(out var cape))
                    {
                        Destroy(cape.lineRenderer);
                        Destroy(cape.CapeCollarOBJ);
                        foreach (var point in cape.capePoints)
                        {
                            Destroy(point);
                        }
                        Destroy(cape);
                    }
                }
                transforming = false;
            }
        }

        public class ILikeToTouchPeople : MonoBehaviour
        {
            public void OnCollisionEnter2D(Collision2D col)
            {
                if (col.gameObject.TryGetComponent<LimbBehaviour>(out var person))
                {
                    GetComponent<LimbBehaviour>().Person.Limbs[0].GetComponent<Transformation>().LastHit = person.Person;
                }
            }
        }
    }

    public class WallClimbing : Power
    {
        public static Power SetPower(PersonBehaviour Person, Sprite icon)
        {
            var power = Person.gameObject.AddComponent<WallClimbing>();
            power.Name = "Wall Climbing";
            power.Description = "Allows the user to climb walls and ceilings by attaching to them with their hands and feet\n<color=\"yellow\">Disabling the power will detach the user from the wall or ceiling, but they can also be removed with slight force";
            power.icon = icon;
            power.targetLimb = TargettedLimb.Internal;

            foreach (var limb in Person.Limbs)
            {
                if (limb.name.Contains("LowerArm"))
                    limb.gameObject.AddComponent<WallClimbLimb>();

                if (limb.name.Contains("Foot"))
                    limb.gameObject.AddComponent<WallClimbLimb>();
            }

            return power;
        }

        public override void DisablePower()
        {
            base.DisablePower();
            foreach (var limb in GetComponent<PersonBehaviour>().Limbs)
            {
                if (limb.GetComponent<WallClimbLimb>())
                {
                    Destroy(limb.GetComponent<WallClimbLimb>().HingeJoint);
                }
            }
        }

        public class WallClimbLimb : MonoBehaviour
        {
            public HingeJoint2D HingeJoint;

            public void OnCollisionEnter2D(Collision2D col)
            {
                if (!HingeJoint && col.relativeVelocity.magnitude > 2 && !col.gameObject.GetComponent<LimbBehaviour>() && transform.root.GetComponent<WallClimbing>().Enabled)
                {
                    HingeJoint = gameObject.AddComponent<HingeJoint2D>();
                    HingeJoint.connectedBody = col.rigidbody;
                    HingeJoint.anchor = Vector2.zero;
                    HingeJoint.connectedAnchor = Vector2.zero;
                    HingeJoint.autoConfigureConnectedAnchor = false;
                    HingeJoint.useLimits = false;
                    HingeJoint.breakForce = 500f;
                }
            }
        }
    }

    public class ThunderBolt : Power
    {
        Sprite HandSprite;
        GameObject ZapThing;
        float Charge;
        public PhysicalBehaviour PhysicalBehaviour;
        public ParticleSystem ChargeUpParticleSystem;
        private Vector3 lastPosition;
        private AudioSource Sound;
        public Color LightningColor = Color.yellow;

        public void OnDisable()
        {
            ZapThing.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            ChargeUpParticleSystem.Stop();
            Sound.Stop();
        }

        public override void DisablePower()
        {
            ZapThing.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            ChargeUpParticleSystem.Stop();
            Sound.Stop();
            base.DisablePower();
        }

        public static Power SetPower(PersonBehaviour person, LimbBehaviour TargetLimb, Color lightningColor, Sprite icon)
        {
            var bolt2 = Instantiate(ModAPI.FindSpawnable("Particle Projector").Prefab.transform.FindChild("bolts").gameObject);
            bolt2.transform.parent = TargetLimb.transform;
            bolt2.name = "notbolt";
            bolt2.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().startColor = (Color)lightningColor;
            bolt2.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().startColor = (Color)lightningColor;
            var boltmain2 = bolt2.GetComponent<ParticleSystem>().main;
            boltmain2.startColor = (Color)lightningColor;
            bolt2.transform.localPosition = Vector2.zero;
            bolt2.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            var power = TargetLimb.gameObject.AddComponent<ThunderBolt>();
            power.LightningColor = lightningColor;
            power.ChargeUpParticleSystem = bolt2.GetComponent<ParticleSystem>();
            power.icon = icon;
            power.Name = "Electric Orb";
            power.Description = "Allows the user to shoot an orb of lightning out of their lower arm\n<color=\"yellow\">The power of the orb depends on how long the activation key is held\nThe arm will glow when it has reached it's full power";
            if (power.name.Contains("Front"))
            {
                power.targetLimb = TargettedLimb.FrontArm;
            }
            else
            {
                power.targetLimb = TargettedLimb.BackArm;
            }
            return power;
        }

        public override void Start()
        {
            PhysicalBehaviour = GetComponent<PhysicalBehaviour>();
            HandSprite = GetComponent<SpriteRenderer>().sprite;

            Sound = gameObject.AddComponent<AudioSource>();
            Sound.GetComponent<AudioSource>().playOnAwake = false;
            Sound.GetComponent<AudioSource>().clip = Mod.Electricity;
            Sound.GetComponent<AudioSource>().spatialBlend = 1;
            Sound.GetComponent<AudioSource>().volume = 1f;
            if (!gameObject.GetComponent<AudioSourceTimeScaleBehaviour>())
            {
                gameObject.AddComponent<AudioSourceTimeScaleBehaviour>();
            }
            ZapThing = new GameObject("ZapCharge");
            ZapThing.transform.parent = transform;
            ZapThing.transform.localPosition = Vector3.zero;
            ZapThing.transform.localRotation = Quaternion.identity;
            ZapThing.transform.localScale = Vector3.one * 1.03f;
            ZapThing.AddComponent<SpriteRenderer>().sprite = HandSprite;
            ZapThing.GetComponent<SpriteRenderer>().sortingLayerName = GetComponent<SpriteRenderer>().sortingLayerName;
            ZapThing.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder - 1;
            ZapThing.GetComponent<SpriteRenderer>().material = ModAPI.FindMaterial("VeryBright");
            ZapThing.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);

            lastPosition = transform.position;
            base.Start();
        }

        private void Update()
        {
            if (Enabled == false)
            {
                return;
            }

            if (PhysicalBehaviour.StartedBeingUsedContinuously())
            {
                Sound.loop = false;
                Sound.PlayOneShot(Mod.Electricity);
                ChargeUpParticleSystem.Play();
                ChargeUpParticleSystem.transform.localScale = Vector3.zero;
                ZapThing.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            }

            if (PhysicalBehaviour.IsBeingUsedContinuously())
            {
                // Calculate movement distance since last frame
                float distanceMoved = Vector3.Distance(transform.position, lastPosition);
                Charge += (Time.deltaTime / 10) + distanceMoved * 0.1f; // Adjust multiplier as needed
                if (!ChargeUpParticleSystem.isPlaying)
                {
                    ChargeUpParticleSystem.Play();
                }
                float num = Time.smoothDeltaTime / 3;

                if (Charge >= 0.9f)
                {
                    ZapThing.GetComponent<SpriteRenderer>().material.SetFloat("_GlowIntensity", 4);
                    ZapThing.GetComponent<SpriteRenderer>().color = LightningColor;
                    ChargeUpParticleSystem.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                }
                else
                {
                    ChargeUpParticleSystem.transform.localScale += (new Vector3(num, num, num) / 3);
                }



                lastPosition = transform.position; // Update last position for next frame
            }

            if (PhysicalBehaviour.StoppedBeingUsedContinuously())
            {
                ChargeUpParticleSystem.transform.localScale = Vector3.zero;
                ChargeUpParticleSystem.Stop();
                Sound.Stop();
                StartCoroutine(PlaySound(Mod.Thunder));
                if (Charge >= 0.9f)
                {
                    ShootFullPower();
                }
                else if (Charge >= 0f)
                {
                    ShootLowPower(Charge * 100);
                }

                Charge = 0f;
                ZapThing.GetComponent<SpriteRenderer>().material.SetFloat("_GlowIntensity", 0);
                ZapThing.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            }
            Charge = Mathf.Clamp01(Charge);
        }

        public void ShootLowPower(float power)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up);
            GameObject purple;
            purple = Instantiate(ModAPI.FindSpawnable("Accelerator").Prefab.gameObject.GetComponent<AcceleratorGunBehaviour>().ProjectilePrefab.gameObject);
            purple.GetComponent<AcceleratorBoltBehaviour>().enabled = false;
            foreach (ParticleSystem par in purple.GetComponentsInChildren<ParticleSystem>())
                par.startColor = LightningColor;
            purple.transform.position = transform.position;
            var bolt3 = Instantiate(ModAPI.FindSpawnable("Particle Projector").Prefab.transform.FindChild("bolts").gameObject);
            bolt3.transform.parent = purple.transform;
            bolt3.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().startColor = LightningColor;
            bolt3.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().startColor = LightningColor;
            var bolt3main2 = bolt3.GetComponent<ParticleSystem>().main;
            bolt3main2.startColor = LightningColor;
            bolt3.transform.localPosition = Vector2.zero;
            bolt3.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            bolt3.GetComponent<ParticleSystem>().Play();
            Vector2 moveDirection = -transform.up;

            StartCoroutine(ShootProjectile(purple, moveDirection, power));
        }

        public void ShootFullPower()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up);
            GameObject purple;
            purple = Instantiate(ModAPI.FindSpawnable("Accelerator").Prefab.gameObject.GetComponent<AcceleratorGunBehaviour>().ProjectilePrefab.gameObject);
            purple.GetComponent<AcceleratorBoltBehaviour>().enabled = false;
            foreach (ParticleSystem par in purple.GetComponentsInChildren<ParticleSystem>())
                par.startColor = LightningColor;
            purple.transform.position = transform.position;
            var bolt2 = Instantiate(ModAPI.FindSpawnable("Particle Projector").Prefab.transform.FindChild("bolts").gameObject);
            bolt2.transform.parent = purple.transform;
            bolt2.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().startColor = LightningColor;
            bolt2.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().startColor = LightningColor;
            var boltmain2 = bolt2.GetComponent<ParticleSystem>().main;
            boltmain2.startColor = LightningColor;
            bolt2.GetComponent<ParticleSystem>().emissionRate *= 50;
            bolt2.transform.localPosition = Vector2.zero;
            bolt2.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            bolt2.GetComponent<ParticleSystem>().Play();
            var bolt3 = Instantiate(ModAPI.FindSpawnable("Particle Projector").Prefab.transform.FindChild("bolts").gameObject);
            bolt3.transform.parent = purple.transform;
            bolt3.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().startColor = LightningColor;
            bolt3.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().startColor = LightningColor;
            var bolt3main2 = bolt3.GetComponent<ParticleSystem>().main;
            bolt3main2.startColor = LightningColor;
            bolt3.transform.localPosition = Vector2.zero;
            bolt3.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            bolt3.GetComponent<ParticleSystem>().Play();
            Vector2 moveDirection = -transform.up;

            StartCoroutine(ShootProjectile(purple, moveDirection, 100));
        }

        private IEnumerator ShootProjectile(GameObject projectile, Vector2 direction, float power)
        {
            float elapsedTime = 0f;
            float shootDuration = 120;
            float moveSpeed = 75;

            while (elapsedTime < shootDuration)
            {

                projectile.transform.Translate(direction * moveSpeed * Time.deltaTime);

                Collider2D[] nearbyObjects = Physics2D.OverlapCircleAll(projectile.transform.position, 5);

                Collider2D[] SuperNearObjects = Physics2D.OverlapCircleAll(projectile.transform.position, 0.5f);

                foreach (var obj in SuperNearObjects)
                {
                    Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();

                    if (rb != null)
                    {
                        if (obj.GetComponent<LimbBehaviour>())
                        {
                            if (obj.GetComponent<LimbBehaviour>().Person != GetComponent<LimbBehaviour>().Person)
                            {

                                var emp = Instantiate(ModAPI.FindSpawnable("EMP Generator").Prefab.GetComponent<EMPBehaviour>().Effect);
                                emp.transform.position = projectile.transform.position;
                                var main = emp.GetComponent<ParticleSystem>().main;
                                main.startColor = LightningColor;
                                emp.transform.GetChild(0).GetComponent<ParticleSystem>().startColor = LightningColor;

                                Collider2D[] nearbyObjects2 = Physics2D.OverlapCircleAll(projectile.transform.position, 25);

                                foreach (var obj2 in nearbyObjects2)
                                {
                                    if (obj2.GetComponent<LimbBehaviour>())
                                    {
                                        if (obj2.GetComponent<LimbBehaviour>().Person != GetComponent<LimbBehaviour>().Person)
                                        {
                                            obj2.GetComponent<PhysicalBehaviour>().charge += power / 20;
                                        }
                                    }
                                    else if (obj2.GetComponent<PhysicalBehaviour>())
                                    {
                                        obj2.GetComponent<PhysicalBehaviour>().charge += power / 20;
                                    }
                                }

                                if (power > 90)
                                {
                                    ExplosionCreator.Explode(projectile.transform.position, power / 15);
                                }
                                else
                                {
                                    ExplosionCreator.CreatePulseExplosion(projectile.transform.position, 2, 15, false, false);
                                    var pf = ModAPI.CreateParticleEffect("IonExplosion", projectile.transform.position).GetComponent<ParticleSystem>();
                                    var mainn = pf.main;
                                    pf.transform.GetChild(1).GetComponent<ParticleSystem>().startColor = LightningColor;
                                    mainn.startColor = LightningColor;
                                }

                                StartCoroutine(PlaySound(Mod.Thunder));
                                Destroy(projectile);
                                yield break;
                            }
                        }
                        else
                        {

                            var emp = Instantiate(ModAPI.FindSpawnable("EMP Generator").Prefab.GetComponent<EMPBehaviour>().Effect);
                            emp.transform.position = projectile.transform.position;
                            var main = emp.GetComponent<ParticleSystem>().main;
                            main.startColor = LightningColor;
                            emp.transform.GetChild(0).GetComponent<ParticleSystem>().startColor = LightningColor;

                            Collider2D[] nearbyObjects2 = Physics2D.OverlapCircleAll(projectile.transform.position, 25);

                            foreach (var obj2 in nearbyObjects2)
                            {
                                if (obj2.GetComponent<LimbBehaviour>())
                                {
                                    if (obj2.GetComponent<LimbBehaviour>().Person != GetComponent<LimbBehaviour>().Person)
                                    {
                                        obj2.GetComponent<PhysicalBehaviour>().charge += .01f;
                                    }
                                }
                                else if (obj2.GetComponent<PhysicalBehaviour>())
                                {
                                    obj2.GetComponent<PhysicalBehaviour>().charge += .01f;
                                }
                            }

                            if (power > 90)
                            {
                                ExplosionCreator.Explode(projectile.transform.position, power / 15);
                            }
                            else
                            {
                                ExplosionCreator.CreatePulseExplosion(projectile.transform.position, 2, 15, false, false);
                                var pf = ModAPI.CreateParticleEffect("IonExplosion", projectile.transform.position).GetComponent<ParticleSystem>();
                                var mainn = pf.main;
                                pf.transform.GetChild(1).GetComponent<ParticleSystem>().startColor = LightningColor;
                                mainn.startColor = LightningColor;
                            }

                            StartCoroutine(PlaySound(Mod.Thunder));
                            Destroy(projectile);
                            yield break;
                        }
                    }
                }

                foreach (var obj in nearbyObjects)
                {
                    Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
                    if (rb != null && !obj.GetComponent<LimbBehaviour>() && !obj.GetComponent<DestroyableBehaviour>() && obj.gameObject.layer == 9)
                    {
                    }
                    else if (rb != null && obj.gameObject.layer == 9)
                    {
                        if (obj.TryGetComponent<PhysicalBehaviour>(out var phys))
                        {
                            if (obj.TryGetComponent<LimbBehaviour>(out var limb))
                            {
                                if (limb.Person != GetComponent<LimbBehaviour>().Person)
                                {
                                    limb.PhysicalBehaviour.charge += .1f;
                                }
                            }
                            else
                            {
                                phys.charge += .1f;
                            }
                        }
                    }
                }

                float shrinkAmount = 1f - (elapsedTime / shootDuration);
                projectile.transform.localScale = new Vector3(shrinkAmount, shrinkAmount, shrinkAmount);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            Destroy(projectile);
        }

        IEnumerator PlaySound(AudioClip SoundToPlay)
        {
            var sound = new GameObject();
            sound.name = "SFX";
            sound.transform.position = gameObject.transform.position;
            sound.AddComponent<AudioSource>();
            sound.GetComponent<AudioSource>().playOnAwake = false;
            sound.GetComponent<AudioSource>().clip = SoundToPlay;
            sound.GetComponent<AudioSource>().spatialBlend = 1;
            sound.GetComponent<AudioSource>().volume = 2f;
            sound.AddComponent<AudioDistortionFilter>().distortionLevel = 0.85f;
            sound.AddComponent<AudioSourceTimeScaleBehaviour>();
            sound.GetComponent<AudioSource>().Play();

            yield return new WaitForSeconds(SoundToPlay.length);

            Destroy(sound);
        }

    }

    public class ShockerArms : Power
    {
        public static Power SetPower(PersonBehaviour Person, LimbBehaviour Limb, Sprite icon)
        {
            var power = Limb.gameObject.AddComponent<ShockerArms>();
            power.Name = "Shocker Arms";
            power.Description = "Allows the user to shoot a shockwave out of their arms on impact with any object\n<color=\"yellow\">The shockwave will knock back any object in front of the user, and charge them briefly";
            power.icon = icon;

            if (power.name.Contains("Front"))
            {
                power.targetLimb = TargettedLimb.FrontArm;
            }
            else
            {
                power.targetLimb = TargettedLimb.BackArm;
            }
            return power;
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (!Enabled)
                return;

            Vector2 relativeVelocity = collision.relativeVelocity;

            float impactStrength = relativeVelocity.magnitude * 2;

            if (collision.gameObject.GetComponent<Rigidbody2D>() && collision.relativeVelocity.magnitude > 3)
            {
                Vector2 directionToThisObject = (GetComponent<Rigidbody2D>().position - collision.rigidbody.position).normalized;

                float dotProduct = Vector2.Dot(relativeVelocity, directionToThisObject);

                if (dotProduct > 0)
                {
                    collision.gameObject.GetComponent<Rigidbody2D>().velocity += -relativeVelocity.normalized * impactStrength;
                    CameraShakeBehaviour.main.Shake(7, base.transform.position);
                    if(collision.gameObject.GetComponent<PhysicalBehaviour>())
                        collision.gameObject.GetComponent<PhysicalBehaviour>().charge += impactStrength / 10;
                    ModAPI.CreateParticleEffect("HugeZap", base.transform.position);
                }

            }
        }
    }

    public class SuperPunch : Power
    {
        public static Power SetPower(PersonBehaviour Person, LimbBehaviour Limb, Sprite icon)
        {
            var power = Limb.gameObject.AddComponent<SuperPunch>();
            power.Name = "Super Punch";
            power.Description = "A punch that can send people flying.";
            power.icon = icon;

            if (power.name.Contains("Front"))
            {
                power.targetLimb = TargettedLimb.FrontArm;
            }
            else
            {
                power.targetLimb = TargettedLimb.BackArm;
            }
            return power;
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (!Enabled)
                return;

            Vector2 relativeVelocity = collision.relativeVelocity;

            float impactStrength = relativeVelocity.magnitude * 2;

            if (collision.gameObject.GetComponent<Rigidbody2D>() && collision.relativeVelocity.magnitude > 3)
            {
                Vector2 directionToThisObject = (GetComponent<Rigidbody2D>().position - collision.rigidbody.position).normalized;

                float dotProduct = Vector2.Dot(relativeVelocity, directionToThisObject);

                if (dotProduct > 0)
                {
                    collision.gameObject.GetComponent<Rigidbody2D>().velocity += -relativeVelocity.normalized * impactStrength;
                    CameraShakeBehaviour.main.Shake(5, base.transform.position);
                }

            }
        }
    }

    public class SmokeFlight : Power, Messages.IUse
    {
        public bool InFlight = false;
        public PersonBehaviour person;
        GameObject Smoke;
        float gravity;
        const float ogbasestrength = 8.5f;

        public static Power SetPower(PersonBehaviour person, LimbBehaviour limb, Sprite icon)
        {
            var power = limb.gameObject.AddComponent<SmokeFlight>();
            power.person = person;
            power.Name = "Smoke Flight";
            power.Description = "Activate to fly with smoke. \n<color=\"yellow\">Activate to start flying \nActivate again to stop flying";
            power.icon = icon;
            if (power.name.Contains("Front"))
            {
                power.targetLimb = TargettedLimb.FrontLeg;
            }
            else
            {
                power.targetLimb = TargettedLimb.BackLeg;
            }
            return power;
        }

        void OnDestroy()
        {
            if (Smoke) Destroy(Smoke);
        }

        public override void Start()
        {
            base.Start();

            Smoke = Instantiate(ModAPI.FindSpawnable("Particle Projector").Prefab.transform.GetChild(0).gameObject);
            var pfx = Smoke.GetComponent<ParticleSystem>();
            pfx.playOnAwake = false;
            pfx.Stop();
            pfx.gravityModifier = 0;
            pfx.startSpeed = 0;
            pfx.startColor = new Color(0, 0.28f, 0, 0.102f);
            Smoke.transform.parent = transform;
            Smoke.transform.localPosition = Vector3.zero;
            Smoke.transform.localRotation = Quaternion.identity;
        }

        public override void DisablePower()
        {
            base.DisablePower();
            if (InFlight) StopThrusting();
        }

        public void FixedUpdate()
        {
            if (!InFlight) return;
            foreach (var limb in person.Limbs)
            {
                if (!Smoke.GetComponent<ParticleSystem>().isPlaying)
                    Smoke.GetComponent<ParticleSystem>().Play();
                var rb2d = limb.GetComponent<Rigidbody2D>();
                if (rb2d.bodyType == RigidbodyType2D.Static) continue;
                rb2d.angularVelocity *= 0.94f;
                rb2d.velocity *= 0.94f;
                if (limb.name.Contains("Body"))
                {
                    var phys = limb.GetComponent<PhysicalBehaviour>();
                    float mass = phys.rigidbody.mass / 1.5f;
                    float torque = 10 * Mathf.Clamp(phys.Charge, 1f, 5f) * mass * mass;
                    phys.rigidbody.angularVelocity *= 0.5f;
                    rb2d.AddTorque(Mathf.DeltaAngle(limb.transform.eulerAngles.z, 0f) * torque);
                }
            }
        }

        public void StopThrusting()
        {
            InFlight = false;
            if (Smoke) Smoke.GetComponent<ParticleSystem>().Stop();
            person.OverridePoseIndex = -1;
            foreach (var limb in person.Limbs)
            {
                if (limb.name.Contains("Arm")) limb.BaseStrength = ogbasestrength;
                var lf = limb.GetComponent<ElectricFlight>();
                if (lf && lf.InFlight) lf.StopThrusting();
                limb.GetComponent<Rigidbody2D>().gravityScale = gravity;
            }
        }

        public void StartThrusting()
        {
            if (InFlight) return;
            person.OverridePoseIndex = 8;
            InFlight = true;
            if (Smoke) Smoke.GetComponent<ParticleSystem>().Play();
            foreach (var limb in person.Limbs)
            {
                if (limb.name.Contains("Arm")) limb.BaseStrength = 0;
                gravity = limb.GetComponent<PhysicalBehaviour>().InitialGravityScale;
                limb.GetComponent<Rigidbody2D>().gravityScale = 0;
            }
        }

        public void Use(ActivationPropagation activation)
        {
            if (!Enabled) return;
            if (!InFlight)
            {
                StartThrusting();
                foreach (var limb in person.Limbs)
                {
                    var lf = limb.GetComponent<ElectricFlight>();
                    if (lf && !lf.InFlight) lf.StartThrusting();
                }
            }
            else
            {
                StopThrusting();
                foreach (var limb in person.Limbs)
                {
                    var lf = limb.GetComponent<ElectricFlight>();
                    if (lf && lf.InFlight) lf.StopThrusting();
                }
            }
        }
    }

    public class ElectricTelekinesis : Power, Messages.IUseContinuous, Messages.IUse
    {
        public float rayDistance = 10f;
        public float forceMagnitude = 10f;
        public LayerMask layerMask;
        public float holdDistance = 4f;
        public float closeDistanceThreshold = 0.1f;

        private LimbBehaviour limb;
        private AudioSource audioSource;
        private LightningGunBehaviour lightningPrefab;
        private LightningGunBehaviour lightning;
        private Transform bolts;
        private Transform searching;

        private GameObject target;

        public static Power SetPower(PersonBehaviour Person, LimbBehaviour Limb, Sprite icon)
        {
            var power = Limb.gameObject.AddComponent<ElectricTelekinesis>();
            power.Name = "Electric Telekinesis";
            power.Description = "Hold the activation key to make any object in from of the user's arm float\n<color =\"yellow\">This power also charges any object held with telekinesis";
            power.icon = icon;

            if (power.name.Contains("Front"))
            {
                power.targetLimb = TargettedLimb.FrontArm;
            }
            else
            {
                power.targetLimb = TargettedLimb.BackArm;
            }
            power.limb = Limb;
            return power;
        }

        public override void DisablePower()
        {
            lightning.ParticleSystem.Stop();
            lightning.LoopSource.Stop();
            lightning.LoopSource.PlayOneShot(lightning.End);
            base.DisablePower();
        }

        public void Use(ActivationPropagation activation)
        {
            if (!Enabled)
                return;

            layerMask = LayerMask.GetMask("Objects");
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, rayDistance, layerMask);

            if (hit.collider != null)
            {
                if (target != hit.collider.gameObject && hit.collider.gameObject.GetComponent<Rigidbody2D>().bodyType != RigidbodyType2D.Static)
                {
                    target = hit.collider.gameObject;
                }
            }
            else
            {
                target = null;
            }
        }

        public override void Start()
        {
            GameObject prefab = ModAPI.FindSpawnable("Stormbaan").Prefab;
            lightningPrefab = prefab.GetComponent<LightningGunBehaviour>();
            bolts = Instantiate(prefab.transform.GetChild(0));

            searching = bolts.transform.GetChild(0);

            foreach (Transform child in bolts)
            {
                if (child.name != searching.name)
                {
                    child.gameObject.SetActive(false);
                }
            }

            lightning = limb.gameObject.GetOrAddComponent<LightningGunBehaviour>();
            lightning.enabled = false;

            lightning.TargetPosition = Instantiate(ModAPI.FindSpawnable("Stormbaan").Prefab.transform.FindChild("target"));

            audioSource = limb.gameObject.AddComponent<AudioSource>();
            audioSource.loop = true;
            limb.gameObject.AddComponent<AudioSourceTimeScaleBehaviour>();
            
            searching.GetChild(0).GetComponent<ParticleSystemRenderer>().materials[1].color = Color.yellow;
            searching.GetChild(0).GetComponent<ParticleSystem>().startColor = Color.yellow;
            searching.GetChild(0).GetComponent<ParticleSystem>().startSize = 0.1f;

            bolts.GetComponent<ParticleSystemRenderer>().materials[1].color = Color.yellow;
            bolts.GetComponent<ParticleSystem>().startColor = Color.yellow;
            bolts.GetComponent<ParticleSystem>().startSize = 0.05f;
            bolts.GetComponent<ParticleSystem>().maxParticles = 25;
            bolts.GetComponent<ParticleSystem>().emissionRate = 10;
            bolts.GetComponent<ParticleSystem>().startLifetime = 1f;
            bolts.GetComponent<ParticleSystem>().startSpeed = 7f;

            lightning.Range = lightningPrefab.Range;
            lightning.Radius = lightningPrefab.Radius;
            lightning.ChargeOnHit = 0f;
            lightning.PushForce = 5f;
            lightning.VibrateIntensity = 0f;
            lightning.CameraShakeIntensity = 0f;
            lightning.IgnitionChance = -1f;
            lightning.PriorityLayer = lightningPrefab.PriorityLayer;
            lightning.Layers = lightningPrefab.Layers;
            lightning.Phys = limb.GetComponent<PhysicalBehaviour>();
            lightning.LoopSource = audioSource;
            lightning.ParticleSystem = bolts.GetComponent<ParticleSystem>();

            base.Start();
        }

        public void Update()
        {
            if (bolts != null)
            {
                bolts.transform.position = limb.transform.position;
                bolts.transform.rotation = limb.transform.rotation;
                bolts.transform.localScale = new Vector3(1, -1f, 1);
            }

            if (Enabled)
            {
                if (lightning.Phys.StartedBeingUsedContinuously())
                {
                    lightning.ParticleSystem.Play();
                    lightning.LoopSource.Play();
                    lightning.LoopSource.PlayOneShot(lightning.Start);
                }

                if (lightning.Phys.StoppedBeingUsedContinuously())
                {
                    lightning.ParticleSystem.Stop();
                    lightning.LoopSource.Stop();
                    lightning.LoopSource.PlayOneShot(lightning.End);
                }
            }
        }

        public void UseContinuous(ActivationPropagation activation)
        {
            if (!Enabled)
                return;

            if (target != null)
            {
                Rigidbody2D rb = target.GetComponent<Rigidbody2D>();



                if (rb != null && rb.bodyType != RigidbodyType2D.Static)
                {
                    lightning.TargetPosition.position = target.transform.position;

                    Vector2 targetPosition = (Vector3)transform.position + (-transform.up * holdDistance);
                    Vector2 direction = targetPosition - rb.position;
                    float distance = direction.magnitude;

                    float adjustedForceMagnitude = Mathf.Lerp(0, (target.GetComponent<Rigidbody2D>().mass) + forceMagnitude, distance / closeDistanceThreshold);
                    adjustedForceMagnitude = Mathf.Clamp(adjustedForceMagnitude, 0, (target.GetComponent<Rigidbody2D>().mass) + forceMagnitude);

                    if (target.GetComponent<LimbBehaviour>())
                    {
                        rb.velocity = direction.normalized * adjustedForceMagnitude * 4;
                    }
                    else
                    {
                        rb.velocity = direction.normalized * adjustedForceMagnitude;
                    }

                    if (rb.TryGetComponent<PhysicalBehaviour>(out var ph))
                    {
                        ph.Charge = 3;
                    }
                }
            }
            else
            {
                layerMask = LayerMask.GetMask("Objects");
                RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, -transform.up, rayDistance, layerMask);
                foreach (var hit in hits)
                {
                    if (hit.collider.GetComponent<LimbBehaviour>())
                    {
                        if (hit.collider.GetComponent<LimbBehaviour>().Person != this.GetComponent<LimbBehaviour>().Person && hit.collider.gameObject.GetComponent<Rigidbody2D>().bodyType != RigidbodyType2D.Static)
                        {
                            this.target = hit.collider.gameObject;
                        }

                    }
                    else if (hit.collider.gameObject.GetComponent<Rigidbody2D>().bodyType != RigidbodyType2D.Static)
                    {
                        this.target = hit.collider.gameObject;
                    }
                }
            }
        }
    }

    public class ElectricFlight : Power, Messages.IUse
    {
        public bool InFlight = false;
        public PersonBehaviour person;
        GameObject Lightning;
        float gravity;
        const float ogbasestrength = 8.5f;

        public static Power SetPower(PersonBehaviour person, LimbBehaviour limb, Sprite icon)
        {
            var power = limb.gameObject.AddComponent<ElectricFlight>();
            power.person = person;
            power.Name = "Electric Flight";
            power.Description = "Activate to fly with lightning bolts. \n<color=\"yellow\">Activate to start flying \nActivate again to stop flying";
            power.icon = icon;
            if (power.name.Contains("Front"))
            {
                power.targetLimb = TargettedLimb.FrontLeg;
            }
            else
            {
                power.targetLimb = TargettedLimb.BackLeg;
            }
            return power;
        }

        void OnDestroy()
        {
            if (Lightning) Destroy(Lightning);
        }

        public override void Start()
        {
            base.Start();

            Lightning = Instantiate(ModAPI.FindSpawnable("Particle Projector").Prefab.transform.Find("bolts").gameObject, transform);
            var mainPS = Lightning.GetComponent<ParticleSystem>();
            mainPS.startColor = Color.yellow;
            mainPS.emissionRate = 50;
            var childPS = Lightning.transform.childCount > 0 ? Lightning.transform.GetChild(0).GetComponent<ParticleSystem>() : null;
            if (childPS) childPS.startColor = Color.yellow;
            Lightning.transform.localPosition = Vector2.zero;
            Lightning.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }

        public override void DisablePower()
        {
            base.DisablePower();
            if (InFlight) StopThrusting();
        }

        public void FixedUpdate()
        {
            if (!InFlight) return;
            foreach (var limb in person.Limbs)
            {
                if(!Lightning.GetComponent<ParticleSystem>().isPlaying)
                    Lightning.GetComponent<ParticleSystem>().Play();
                var rb2d = limb.GetComponent<Rigidbody2D>();
                if (rb2d.bodyType == RigidbodyType2D.Static) continue;
                rb2d.angularVelocity *= 0.94f;
                rb2d.velocity *= 0.94f;
                if (limb.name.Contains("Body"))
                {
                    var phys = limb.GetComponent<PhysicalBehaviour>();
                    float mass = phys.rigidbody.mass / 1.5f;
                    float torque = 10 * Mathf.Clamp(phys.Charge, 1f, 5f) * mass * mass;
                    phys.rigidbody.angularVelocity *= 0.5f;
                    rb2d.AddTorque(Mathf.DeltaAngle(limb.transform.eulerAngles.z, 0f) * torque);
                }
            }
        }

        public void StopThrusting()
        {
            InFlight = false;
            if (Lightning) Lightning.GetComponent<ParticleSystem>().Stop();
            person.OverridePoseIndex = -1;
            foreach (var limb in person.Limbs)
            {
                if (limb.name.Contains("Arm")) limb.BaseStrength = ogbasestrength;
                var lf = limb.GetComponent<ElectricFlight>();
                if (lf && lf.InFlight) lf.StopThrusting();
                limb.GetComponent<Rigidbody2D>().gravityScale = gravity;
            }
        }

        public void StartThrusting()
        {
            if (InFlight) return;
            person.OverridePoseIndex = 8;
            InFlight = true;
            if (Lightning) Lightning.GetComponent<ParticleSystem>().Play();
            foreach (var limb in person.Limbs)
            {
                if (limb.name.Contains("Arm")) limb.BaseStrength = 0;
                gravity = limb.GetComponent<PhysicalBehaviour>().InitialGravityScale;
                limb.GetComponent<Rigidbody2D>().gravityScale = 0;
            }
        }

        public void Use(ActivationPropagation activation)
        {
            if (!Enabled) return;
            if (!InFlight)
            {
                StartThrusting();
                foreach (var limb in person.Limbs)
                {
                    var lf = limb.GetComponent<ElectricFlight>();
                    if (lf && !lf.InFlight) lf.StartThrusting();
                }
            }
            else
            {
                StopThrusting();
                foreach (var limb in person.Limbs)
                {
                    var lf = limb.GetComponent<ElectricFlight>();
                    if (lf && lf.InFlight) lf.StopThrusting();
                }
            }
        }
    }

    public class ElectricAura : Power, Messages.IUse
    {
        public bool FlameIsOn = false;
        public List<GameObject> GlowObjects = new List<GameObject>();
        bool CanSwitch = true;
        public Color32 color;

        public float outlineOffset = 0.01f;

        Vector2[] offsets = new Vector2[]
        {
        new Vector2(-1, 0),
        new Vector2(1, 0),
        new Vector2(0, -1),
        new Vector2(0, 1),
        new Vector2(-1, -1),
        new Vector2(-1, 1),
        new Vector2(1, -1),
        new Vector2(1, 1)
        };

        public static Power SetPower(PersonBehaviour Person, LimbBehaviour Limb, Sprite icon)
        {
            var power = Limb.gameObject.AddComponent<ElectricAura>();
            power.Name = "Electric Aura";
            power.Description = "When the head is activated the user will become enveloped in an electrical yellow aura<color=\"yellow\">\nThis power also makes the user impervious to electricity, and charges anything touching it.";
            power.icon = icon;
            power.targetLimb = TargettedLimb.Head;

            foreach (var limb in Person.Limbs)
            {
                var newprops = GameObject.Instantiate(limb.PhysicalBehaviour.Properties);
                newprops.Conducting = false;
                limb.PhysicalBehaviour.Properties = newprops;
                power.color = Color.yellow;
                power.CreateOutlineGlow(limb.gameObject, limb.GetComponent<SpriteRenderer>().sprite, limb.GetComponent<SpriteRenderer>().sortingLayerName, limb.GetComponent<SpriteRenderer>().sortingOrder - 2, new Color(1f, 47f / 51f, 0.0156862754f, 0f), "TenShroud");
            }

            foreach (var limb in Person.Limbs)
            {
                limb.PhysicalBehaviour.ForceNoCharge = true;

                foreach (var limbb in Person.Limbs)
                {
                    var newprops = GameObject.Instantiate(limb.PhysicalBehaviour.Properties);
                    newprops.Conducting = false;
                    limb.PhysicalBehaviour.Properties = newprops;
                    power.color = Color.yellow;
                }

                var bolt = Instantiate(ModAPI.FindSpawnable("Particle Projector").Prefab.transform.FindChild("bolts").gameObject);
                bolt.transform.parent = limb.transform;
                bolt.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().startColor = Color.yellow;
                bolt.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().startColor = Color.yellow;
                var boltmain = bolt.GetComponent<ParticleSystem>().main;
                boltmain.startColor = Color.yellow;
                bolt.transform.localPosition = Vector2.zero;
                bolt.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            }

            return power;
        }

        void CreateOutlineGlow(GameObject limb, Sprite sprite, string sortingLayer, int sortingOrder, Color color, string name)
        {
            foreach (var off in offsets)
            {
                Vector3 localPos = new Vector3(off.x * outlineOffset, off.y * outlineOffset, 0f);

                GameObject glow = new GameObject(name);
                glow.transform.SetParent(limb.transform);
                glow.transform.localPosition = localPos;
                glow.transform.localRotation = Quaternion.identity;
                glow.transform.localScale = Vector3.one;

                SpriteRenderer sr = glow.AddComponent<SpriteRenderer>();
                sr.sprite = sprite;
                sr.sortingLayerName = sortingLayer;
                sr.sortingOrder = sortingOrder;
                sr.material = ModAPI.FindMaterial("VeryBright");
                sr.color = color;

                GlowObjects.Add(glow);
            }
        }

        public void OnCollisionEnter2D(Collision2D col)
        {
            if (col.collider.TryGetComponent<PhysicalBehaviour>(out var phys) && col.relativeVelocity.magnitude > 3 && FlameIsOn)
            {
                phys.charge += 10;
            }
        }

        public void Use(ActivationPropagation activation)
        {
            if (Enabled && CanSwitch)
            {
                foreach (var flame in GlowObjects)
                {
                    Destroy(flame);
                }
                GlowObjects.Clear();

                foreach (var limb in GetComponent<LimbBehaviour>().Person.Limbs)
                {
                    CreateOutlineGlow(limb.gameObject, limb.GetComponent<SpriteRenderer>().sprite, limb.GetComponent<SpriteRenderer>().sortingLayerName, limb.GetComponent<SpriteRenderer>().sortingOrder - 1, new Color32(color.r, color.g, color.b, 0), "GlowFlame");
                }

                if (FlameIsOn)
                {
                    foreach (var bolt in GetComponent<LimbBehaviour>().Person.transform.GetComponentsInChildren<ParticleSystem>())
                    {
                        if (bolt.name == "bolts(Clone)")
                        {
                            bolt.Stop();
                        }
                    }
                    foreach (var glow in GlowObjects)
                    {
                        StartCoroutine(ColorOverTime(glow, new Color32(color.r, color.g, color.b, 0), new Color32(color.r, color.g, color.b, 15)));
                    }
                    FlameIsOn = false;
                }
                else
                {
                    foreach (var bolt in GetComponent<LimbBehaviour>().Person.transform.GetComponentsInChildren<ParticleSystem>())
                    {
                        if (bolt.name == "bolts(Clone)")
                        {
                            bolt.Play();
                        }
                    }
                    foreach (var glow in GlowObjects)
                    {
                        StartCoroutine(ColorOverTime(glow, new Color32(color.r, color.g, color.b, 15), new Color32(color.r, color.g, color.b, 0)));
                    }
                    FlameIsOn = true;
                }
            }
        }

        public override void DisablePower()
        {
            if (FlameIsOn)
            {
                foreach (var bolt in GetComponent<LimbBehaviour>().Person.transform.GetComponentsInChildren<ParticleSystem>())
                {
                    if (bolt.name == "bolts(Clone)")
                    {
                        bolt.Stop();
                    }
                }
                foreach (var glow in GlowObjects)
                {
                    StartCoroutine(ColorOverTime(glow, new Color32(color.r, color.g, color.b, 0), new Color32(color.r, color.g, color.b, 15)));
                }
            }

            base.DisablePower();
        }

        public void OnDestroy()
        {
            foreach (var bolt in GetComponent<LimbBehaviour>().Person.transform.GetComponentsInChildren<ParticleSystem>())
            {
                if (bolt.name == "bolts(Clone)")
                {
                    bolt.Stop();
                }
            }
            foreach (var glow in GlowObjects)
            {
                Destroy(glow);
            }
        }

        IEnumerator ColorOverTime(GameObject AffectedObject, Color newColor, Color OriginalColor)
        {
            CanSwitch = false;
            var duration = 1f;
            float elapsedTime = 0;

            SpriteRenderer sr = AffectedObject.GetComponent<SpriteRenderer>();
            while (elapsedTime < duration)
            {
                sr.color = Color.Lerp(OriginalColor, newColor, (elapsedTime / duration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            sr.color = newColor;
            CanSwitch = true;
        }
    }

    public class Grabber : MonoBehaviour, Messages.IUse
    {
        public bool CanGrab;
        public Rigidbody2D Overlapping;
        public FixedJoint2D joint;

        public void Use(ActivationPropagation activation)
        {
            if (joint != null){
                Detach();
                return;
            }

            if (CanGrab && Overlapping) Grab();
        }
        public void Detach(){
            GetComponent<SpriteRenderer>().sprite = Mod.GripperOpen;
            GetComponent<PhysicalBehaviour>().RefreshOutline();
            Destroy(joint);
            foreach (var limb in transform.root.GetComponent<PersonBehaviour>().Limbs)
            {
                var limbCol = limb.GetComponent<Collider2D>();
                Physics2D.IgnoreCollision(limbCol, GetComponent<Collider2D>(), true);
            }
        }
        public void Grab(){
            GetComponent<SpriteRenderer>().sprite = Mod.Gripper;
            GetComponent<PhysicalBehaviour>().RefreshOutline();
            Timtam.CreateCollider(GetComponent<SpriteRenderer>());
            joint = gameObject.AddComponent<FixedJoint2D>();
            joint.connectedBody = Overlapping;
            joint.enableCollision = false;
            joint.breakForce = 70000 * Overlapping.mass;
            foreach (var limb in transform.root.GetComponent<PersonBehaviour>().Limbs)
            {
                var limbCol = limb.GetComponent<Collider2D>();
                Physics2D.IgnoreCollision(limbCol, GetComponent<Collider2D>(), true);
            }
        }

        public void OnCollisionStay2D(Collision2D col)
        {
            if (col.gameObject.TryGetComponent<Rigidbody2D>(out var rb) && joint == null)
            {
                Overlapping = rb;
                CanGrab = true;
            }
        }

        public void OnCollisionExit2D(Collision2D col)
        {
            CanGrab = false;
        }
    }

    public class PoisonGiver : MonoBehaviour
    {
        List<PersonBehaviour> affected = new List<PersonBehaviour>();
        public FixedJoint2D joint;

        public void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.TryGetComponent<LimbBehaviour>(out var limb))
            {

                if (col.relativeVelocity.magnitude > 2)
                {
                    bool washealing = false;
                    if (limb.Person.TryGetComponent<SpeedHealing>(out var sped))
                    {
                        if (sped.Enabled)
                        {
                            washealing = true;
                            sped.DisablePower();
                        }
                    }
                        

                    var splat = ModAPI.CreateParticleEffect("BloodExplosion", col.transform.position);
                    if (joint == null)
                    {
                        joint = gameObject.AddComponent<FixedJoint2D>();
                        joint.connectedBody = col.gameObject.GetComponent<Rigidbody2D>();
                        joint.breakForce = 75;
                        joint.breakTorque = 75;
                        joint.enableCollision = true;
                    }
                    foreach (ParticleSystem sys in splat.GetComponentsInChildren<ParticleSystem>())
                    {
                        sys.startColor = new Color(0.1f, 0.5f, 0.1f, 0.5f);
                    }
                    splat.transform.localScale = Vector3.one * 0.5f;
                    if (!affected.Contains(limb.Person))
                    {
                        affected.Add(limb.Person);
                        limb.CirculationBehaviour.AddLiquid(Liquid.GetLiquid("ACID"), 0.5f);
                        StartCoroutine(Fear(limb.Person, washealing));
                    }
                }
            }
        }

        public void FixedUpdate()
        {
            if (joint != null)
            {
                joint.connectedBody.GetComponent<CirculationBehaviour>().AddLiquid(Liquid.GetLiquid("ACID"), 0.001f);
            }
        }

        public IEnumerator Fear(PersonBehaviour person, bool wasHealing)
        {
            person.OverridePoseIndex = 5;
            person.ActivePose.AnimationSpeedMultiplier = 3;
            person.ActivePose.ShouldStandUpright = true;
            yield return new WaitForSeconds(5);

            person.ActivePose.AnimationSpeedMultiplier = 1;
            person.OverridePoseIndex = -1;

            if (wasHealing)
            {
                yield return new WaitForSeconds(20);

                person.GetComponent<SpeedHealing>().EnablePower();
            }

            affected.Remove(person);
        }
    }

    public class TailMover : MonoBehaviour
    {
        public List<GameObject> tailParts;
        public GameObject body;
        bool done;
        public void Start()
        {

            foreach (var limb in body.GetComponent<LimbBehaviour>().Person.Limbs)
            {
                foreach (var col in GetComponents<Collider2D>())
                {
                    Physics2D.IgnoreCollision(limb.GetComponent<Collider2D>(), col, true);
                }
            }
        }

        public void Update()
        {
            if (SelectionController.Main.SelectedObjects.Count == 1 && SelectionController.Main.SelectedObjects[0] == GetComponent<PhysicalBehaviour>() && Input.GetMouseButton(0))
                foreach (var tailpart in tailParts)
                {
                    done = false;
                    if (tailpart.GetComponent<PoisonGiver>() || tailpart.GetComponent<Grabber>())
                    {
                        if (tailpart.TryGetComponent<FixedJoint2D>(out var fix))
                            if (tailpart.TryGetComponent<PoisonGiver>(out var po))
                            {
                                if(fix != po.joint)
                                    Destroy(fix);
                            }else
                            {
                                if(fix != tailpart.GetComponent<Grabber>().joint)
                                    Destroy(fix);
                            }
                    }
                    else
                    {
                        if(!tailpart.GetComponent<Rigidbody2D>().simulated)
                            //tailpart.GetComponent<Rigidbody2D>().simulated = true;
                        if (tailpart.TryGetComponent<DistanceJoint2D>(out var dis))
                        {
                            dis.distance = 0.085f;
                        }
                    }
                }
            else if(!done)
            {
                if (!tailParts[1].GetComponent<FixedJoint>())
                {
                    done = true;
                    StartCoroutine(enumerator());
                }
            }

        }

        public IEnumerator enumerator()
        {
            yield return new WaitForSeconds(0.1f);
            foreach (var tailpart in tailParts)
            {
                //tailpart.AddComponent<FixedJoint2D>().connectedBody = body.GetComponent<Rigidbody2D>();
                if (tailpart.TryGetComponent<Rigidbody2D>(out var dis) && !tailpart.GetComponent<PoisonGiver>() && !tailpart.GetComponent<Grabber>())
                {
                    //dis.simulated = false;
                }else
                {
                    dis.gameObject.AddComponent<FixedJoint2D>().connectedBody = body.GetComponent<Rigidbody2D>();
                }
            }
        }
    }

    public class ArmMover : MonoBehaviour, Mod.ModAPIPlus.IUse2
    {
        public List<GameObject> tailParts;
        public GameObject body;
        public List<Arm> EndParts;
        public EndPartsBehaviour[] EPBs;
        public int Order = 0;
        public bool AI = true;
        public Rigidbody2D rb;

        public void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            StartCoroutine(InitializeArmMover());
        }
        public void Use2(){
            AI = !AI;
            var text = new GameObject("TentaclesModeText");
            var sr = text.AddComponent<SpriteRenderer>();
            sr.sortingLayerName = "Top";
            sr.sortingOrder = 1000;
            text.transform.position = transform.position;
            text.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            text.AddComponent<FakeNotifDissapearer>();
            if (AI) sr.sprite = Mod.AI;
            else sr.sprite = Mod.Manual;
        }
        private IEnumerator InitializeArmMover()
        {
            var allColliders = GetComponentsInChildren<Collider2D>();
            var limbs = body.GetComponent<LimbBehaviour>().Person.Limbs;

            foreach (var limb in limbs)
            {
                var limbCol = limb.GetComponent<Collider2D>();
                foreach (var col in allColliders)
                {
                    Physics2D.IgnoreCollision(limbCol, col, true);
                }
            }

            for (int i = 0; i < allColliders.Length; i++)
            {
                for (int j = i + 1; j < allColliders.Length; j++)
                {
                    Physics2D.IgnoreCollision(allColliders[i], allColliders[j], true);
                }
            }

            yield return null;

            EndParts = new List<Arm>();
            tailParts = new List<GameObject>();
            var allRigidbodies = GetComponentsInChildren<Rigidbody2D>();

            foreach (var rb in allRigidbodies)
            {
                if (rb.GetComponent<Grabber>())
                    EndParts.Add(Arm.InitializeArm(rb.gameObject));
                if (rb.name.Contains("CapePoint"))
                    tailParts.Add(rb.gameObject);
            }

            EPBs = new EndPartsBehaviour[EndParts.Count];

            for (int i = 0; i < EndParts.Count; ++i){
                EndPartsBehaviour EPB = EndParts[i].arm.gameObject.AddComponent<EndPartsBehaviour>();
                EPB.endpart = EndParts[i];
                EPB.Root = this;
                EPB.index = i;
                EPBs[i] = EPB;
            }
        }

        public void FixedUpdate()
        {
            if (SelectionController.Main.SelectedObjects.Count == 1 && SelectionController.Main.SelectedObjects[0] == GetComponent<PhysicalBehaviour>() && Input.GetMouseButton(0))
            {
                for (int i = 0; i < EPBs.Length; ++i)
                {
                    EPBs[i].grabbing = true;
                }
            }else{
                for (int i = 0; i < EPBs.Length; ++i)
                {
                    EPBs[i].grabbing = false;
                    if (!EPBs[i].endpart.done){
                        EPBs[i].endpart.done = true;
                        StartCoroutine(Enumerator(EPBs[i].endpart.arm));
                    }
                }
            }
            if (AI){
                for (int i = 0; i < EndParts.Count; ++i){
                    EPBs[i].SearchForObjects();
                }
            }
        }
        public class EndPartsBehaviour : MonoBehaviour, Mod.ModAPIPlus.IUse2{
            public static float Force = 80f;
            public static int DirDiv = 36;
            public static float DistanceBetweenArms = 1f;
            public Arm endpart;
            public ArmMover Root;
            public bool grabbing = false;
            public int index;
            private bool free = false;
            private float Range;
            private bool launching = false;
            private PhysicalBehaviour PB;
            private Grabber grab;
            private PersonBehaviour Person;
            private bool selected = false;
            private bool Locked = false;
            public void Use2(){
                Locked = !Locked;
                var text = new GameObject("EPModeText");
                var sr = text.AddComponent<SpriteRenderer>();
                sr.sortingLayerName = "Top";
                sr.sortingOrder = 1000;
                text.transform.position = transform.position;
                text.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                text.AddComponent<FakeNotifDissapearer>();
                if (Locked) sr.sprite = Mod.Locked;
                else sr.sprite = Mod.Unlocked;
            }
            private GameObject Target;
            public void Start(){
                PB = GetComponent<PhysicalBehaviour>();
                grab = GetComponent<Grabber>();
                Person = transform.root.GetComponent<PersonBehaviour>();

                Range = transform.parent.GetComponent<Tail>().numberOfPoints / 10f;

                PB.SoundVolumeBoost = 0f;

                ClickAS = gameObject.AddComponent<AudioSource>();
                ClickAS.outputAudioMixerGroup = Global.main.SoundEffects;
                Global.main.AddAudioSource(ClickAS, false);
                ClickAS.clip = Mod.OctopusWav;
                ClickAS.playOnAwake = false;
                ClickAS.loop = true;

                StompAS = gameObject.AddComponent<AudioSource>();
                StompAS.outputAudioMixerGroup = Global.main.SoundEffects;
                Global.main.AddAudioSource(StompAS, false);
                StompAS.playOnAwake = false;
                StompAS.loop = false;

                PB.ContextMenuOptions.Buttons.Add(new ContextMenuButton("ottoselecttarget", "Select Target", "Select Target", ()=>{
                    DialogBoxManager.Dialog("Target Type", new DialogButton[]{
                        new DialogButton("Everything", true, ()=>Target=null),
                        new DialogButton("Pick One", true, ()=>Functions.CreatePicker<PhysicalBehaviour>(i=>Target=i.gameObject))
                    });
                }));
            }
            private AudioSource StompAS;
            private AudioSource ClickAS;
            public void FixedUpdate(){
                selected = SelectionController.Main.SelectedObjects.Count == 1 && SelectionController.Main.SelectedObjects[0] == PB && Input.GetMouseButton(0);
                if ( ((selected||free||grabbing) && !Locked) || (selected && Locked) ){
                    if (!ClickAS.isPlaying) ClickAS.Play();
                    endpart.done = false;
                    foreach (var fix in endpart.arm.GetComponents<FixedJoint2D>())
                        if (fix != endpart.arm.GetComponent<Grabber>().joint)
                            Destroy(fix);
                }
                else if (!endpart.done){
                    ClickAS.Stop();
                    endpart.done = true;
                    StartCoroutine(Root.Enumerator(endpart.arm));
                }else ClickAS.Stop();

                if (Root.AI && !selected && !Locked){
                    if (Vector3.Distance(transform.position, Person.Limbs[3].transform.position) > Range && grab.joint){
                        grab.Detach();
                        Reset();
                    }
                }
            }
            public void SearchForObjects(){
                if (grab.joint || selected || Locked) return;
                Vector3 BestDir = Vector3.zero;
                float MinD = float.PositiveInfinity;
                for (float i=0; i<360; i+=360f/DirDiv){
                    Vector3 Dir = DegToVec(i);
                    IEnumerable<RaycastHit2D> Hit = Physics2D.RaycastAll(transform.position, Dir, Range*4f).Where(a => a.transform.root != transform.root && Vector3.Distance(a.point, Root.transform.position)<=Range && (Target == null || a.transform==Target.transform));
                    if (Hit.Count()==0) continue;
                    bool yas = true;
                    for (int j = 0; j < Root.EPBs.Length; ++j){
                        if (Root.EPBs[j]==this || Root.EPBs[j].grab.joint==null) continue;
                        if (Vector3.Distance(Root.EPBs[j].transform.position, Hit.First().point)<DistanceBetweenArms) yas = false;
                    }
                    if (!yas) continue;
                    if (Hit.First().distance < MinD){
                        MinD = Hit.First().distance;
                        BestDir = Dir;
                    }
                }
                if (!float.IsPositiveInfinity(MinD)){
                    ClickAS.Play();
                    launching = true;
                    free = true;
                    PB.rigidbody.AddForce(BestDir * Force);
                }
            }
            public static Vector3 DegToVec(float Deg){
                float radian = Deg * Mathf.Deg2Rad;
                return new Vector3(Mathf.Cos(radian), Mathf.Sin(radian), 0f);
            }
            public void OnCollisionEnter2D(Collision2D other){
                if (launching){
                    grab.Overlapping = other.rigidbody;
                    grab.Grab();
                    if (ClickAS.isPlaying) ClickAS.Stop();
                    if (StompAS.isPlaying) StompAS.Stop();
                    StompAS.clip = Mod.OctopusStomp.PickRandom();
                    StompAS.pitch = UnityEngine.Random.Range(.8f, 1.2f);
                    StompAS.Play();
                    CameraShakeBehaviour.main.Shake(3f, transform.position, 10f);
                    Reset();
                }
            }
            public void Reset(){
                launching = false;
                free = false;
                endpart.done = true;
                StartCoroutine(Root.Enumerator(endpart.arm));
            }
        }

        public class Arm
        {
            public GameObject arm;
            public bool done;

            public static Arm InitializeArm(GameObject aarm)
            {
                var arm = new Arm();
                arm.arm = aarm;
                arm.done = false;
                return arm;
            }
        }

        public IEnumerator Enumerator(GameObject tailpart)
        {
            yield return new WaitForSeconds(0.05f);
            if (GetComponent<FixedJoint2D>())
            {
                foreach (var fix in tailpart.GetComponents<FixedJoint2D>())
                    if (fix != tailpart.GetComponent<Grabber>().joint)
                        tailpart.gameObject.AddComponent<FixedJoint2D>().connectedBody = body.GetComponent<Rigidbody2D>();
            }
            else
            {
                tailpart.gameObject.AddComponent<FixedJoint2D>().connectedBody = body.GetComponent<Rigidbody2D>();
            }
        }
    }

    public class DocOckArms
    {
        public static void CreateArmsForPerson(PersonBehaviour person, Texture2D capeTexture)
        {
            const int tailsCount = 4;
            var allColliders = new List<Collider2D>();

            Vector3 baseOffset = new Vector3(0.12f, 0, 0);
            int numberOfPoints = 50;
            float pointSpacing = 0.1f;
            float sizeMultiplier = 1.0f;

            for (int i = 0; i < tailsCount; i++)
            {
                float angle = 360f / tailsCount * i;

                GameObject tailRoot = new GameObject($"Tail_{i}");
                tailRoot.transform.parent = person.Limbs[3].transform;
                tailRoot.transform.localPosition = Vector3.zero;
                tailRoot.transform.localRotation = Quaternion.identity;

                Tail tail = tailRoot.AddComponent<Tail>();
                tail.numberOfPoints = numberOfPoints;
                tail.pointSpacing = pointSpacing;
                tail.sizeMultiplier = sizeMultiplier;
                tail.offset = Vector3.zero;
                tail.capeTexture = capeTexture;

                InitializeTail(tail, person);

                foreach (var point in tail.capePoints)
                {
                    var coll = point.GetComponent<Collider2D>();
                    if (coll != null)
                        allColliders.Add(coll);
                }

                tailRoot.transform.localScale = new Vector3(1, 1, 1);
            }

            var armmover = person.Limbs[3].gameObject.AddComponent<ArmMover>();
            armmover.body = person.Limbs[3].gameObject;

            for (int a = 0; a < allColliders.Count; a++)
            {
                for (int b = a + 1; b < allColliders.Count; b++)
                {
                    Physics2D.IgnoreCollision(allColliders[a], allColliders[b], true);
                }
            }
        }

        private static void InitializeTail(Tail cape, PersonBehaviour person)
        {
            var lr = cape.gameObject.AddComponent<LineRenderer>();
            cape.lineRenderer = lr;
            lr.positionCount = cape.numberOfPoints;
            lr.startWidth = 0.175f * cape.sizeMultiplier;
            lr.endWidth = 0.175f * cape.sizeMultiplier;
            lr.useWorldSpace = false;
            lr.alignment = LineAlignment.View;

            Material material = new Material(Shader.Find("Sprites/Default"));
            material.mainTexture = cape.capeTexture;
            lr.material = material;
            lr.startColor = Color.white;
            lr.endColor = Color.white;

            cape.capePoints = new GameObject[cape.numberOfPoints];
            cape.positions = new Vector3[cape.numberOfPoints];

            for (int j = 0; j < cape.numberOfPoints; j++)
            {
                GameObject point = new GameObject("CapePoint" + j);
                point.transform.parent = cape.transform;
                point.transform.localPosition = (j == 0)
                    ? cape.offset
                    : cape.offset + new Vector3(-j * cape.pointSpacing * cape.sizeMultiplier, 0, 0);

                var rb = point.AddComponent<Rigidbody2D>();
                rb.gravityScale = -1f;
                rb.mass = (j == cape.numberOfPoints - 1) ? 0.03f : 0.005f;
                rb.drag = 2f;
                rb.angularDrag = 0f;

                CircleCollider2D collider = point.AddComponent<CircleCollider2D>();
                collider.radius = 0.1f * cape.sizeMultiplier;

                if (j == cape.numberOfPoints - 1)
                {
                    point.layer = 9;
                    var sr = point.AddComponent<SpriteRenderer>();
                    sr.sprite = Mod.GripperOpen;
                    sr.sortingOrder = 5;
                    point.transform.localScale = new Vector3(-1, 1, 1);
                    var pb = point.AddComponent<PhysicalBehaviour>();
                    pb.Properties = ModAPI.FindPhysicalProperties("Weapon");
                    pb.OverrideShotSounds = Array.Empty<AudioClip>();
                    pb.OverrideImpactSounds = Array.Empty<AudioClip>();
                    pb.SpawnSpawnParticles = false;
                    point.AddComponent<AudioSourceTimeScaleBehaviour>();
                    point.AddComponent<Grabber>();
                    point.GetComponent<Rigidbody2D>().gravityScale = 10f;
                    Timtam.CreateCollider(point.GetComponent<SpriteRenderer>());
                    CircleCollider2D.Destroy(collider);
                    point.transform.localPosition += new Vector3(1f, 0, 0);
                }

                    cape.capePoints[j] = point;
                cape.positions[j] = point.transform.localPosition;
            }

            for (int j = 0; j < cape.numberOfPoints - 1; j++)
            {
                var dj = cape.capePoints[j].AddComponent<DistanceJoint2D>();
                dj.connectedBody = cape.capePoints[j + 1].GetComponent<Rigidbody2D>();
                dj.autoConfigureDistance = false;
                dj.maxDistanceOnly = true;
                dj.distance = cape.pointSpacing * cape.sizeMultiplier;
                cape.capePoints[j + 1].GetComponent<Rigidbody2D>().interpolation = RigidbodyInterpolation2D.Interpolate;
                var a = cape.capePoints[j].gameObject;
            }

            if (cape.GetComponent<Rigidbody2D>() == null)
                cape.gameObject.AddComponent<Rigidbody2D>().simulated = false;
            var firstJoint = cape.capePoints[0].AddComponent<FixedJoint2D>();
            firstJoint.connectedBody = cape.transform.parent.GetComponent<Rigidbody2D>();
        }
    }

    public class Tail : MonoBehaviour
    {
        public int numberOfPoints = 20;
        public float pointSpacing = 0.085f;
        public Vector3 offset = new Vector3(-0.13f, 0.12f, 0);
        public float sizeMultiplier = 1.0f;
        public LineRenderer lineRenderer;
        public GameObject[] capePoints;
        public Vector3[] positions;
        public Texture2D capeTexture;

        public static void CreateTailForPerson(PersonBehaviour person, Texture2D Cape)
        {
            var cape = person.Limbs[3].gameObject.AddComponent<Tail>();
            cape.capeTexture = Cape;
            cape.lineRenderer = cape.gameObject.AddComponent<LineRenderer>();
            cape.lineRenderer.positionCount = cape.numberOfPoints;
            cape.lineRenderer.startWidth = 0.175f * cape.sizeMultiplier;
            cape.lineRenderer.endWidth = 0.175f * cape.sizeMultiplier;

            if (cape.capeTexture != null)
            {
                Material material = new Material(Shader.Find("Sprites/Default"));
                material.mainTexture = cape.capeTexture;
                cape.lineRenderer.material = material;
                cape.lineRenderer.startColor = Color.white;
                cape.lineRenderer.endColor = Color.white;
            }
            else
            {
                cape.lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                cape.lineRenderer.startColor = Color.red;
                cape.lineRenderer.endColor = Color.red;
            }

            cape.lineRenderer.useWorldSpace = false;
            cape.lineRenderer.alignment = LineAlignment.View;

            cape.capePoints = new GameObject[cape.numberOfPoints];
            cape.positions = new Vector3[cape.numberOfPoints];

            for (int i = 0; i < cape.numberOfPoints; i++)
            {
                GameObject point = new GameObject("CapePoint" + i);
                point.transform.parent = cape.transform;
                if (i == 0)
                {
                    point.transform.localPosition = cape.offset;
                }
                else
                {
                    point.transform.localPosition = cape.offset + new Vector3(-i * cape.pointSpacing * cape.sizeMultiplier, 0, 0);
                }


                Rigidbody2D rb = point.AddComponent<Rigidbody2D>();
                rb.gravityScale = 1f;
                rb.mass = 0.005f;
                rb.drag = 2f;
                rb.angularDrag = 0;
                CircleCollider2D collider = point.AddComponent<CircleCollider2D>();
                collider.radius = 0.1f * cape.sizeMultiplier;

                cape.capePoints[i] = point;
                cape.positions[i] = point.transform.localPosition;
            }
            FixedJoint2D firstJoint = cape.capePoints[0].AddComponent<FixedJoint2D>();
            firstJoint.connectedBody = cape.GetComponent<Rigidbody2D>();
            cape.capePoints[19].layer = 9;
            cape.capePoints[19].AddComponent<SpriteRenderer>().sprite = Mod.TailEnd;
            cape.capePoints[19].GetComponent<SpriteRenderer>().sortingOrder = 5;
            cape.capePoints[19].transform.localScale = new Vector3(-1, 1, 1);
            var physicalBehaviour = cape.capePoints[19].AddComponent<PhysicalBehaviour>();
            physicalBehaviour.Properties = ModAPI.FindPhysicalProperties("Weapon");
            physicalBehaviour.SpawnSpawnParticles = false;
            physicalBehaviour.gameObject.AddComponent<AudioSourceTimeScaleBehaviour>();
            physicalBehaviour.OverrideShotSounds = Array.Empty<AudioClip>();
            physicalBehaviour.OverrideImpactSounds = Array.Empty<AudioClip>();
            cape.capePoints[19].GetComponent<Rigidbody2D>().mass = 0.03f;
            cape.capePoints[19].AddComponent<TailMover>().tailParts = new List<GameObject>();
            cape.capePoints[19].AddComponent<PoisonGiver>();
            cape.capePoints[19].GetComponent<TailMover>().body = person.Limbs[3].gameObject;
            cape.capePoints[19].AddComponent<PolygonCollider2D>();
            Destroy(cape.capePoints[19].GetComponent<CircleCollider2D>());

            foreach (var point in cape.capePoints)
            {
                if (point.GetComponent<TailMover>())
                {
                    foreach (var point2 in cape.capePoints)
                    {
                        point.GetComponent<TailMover>().tailParts.Add(point2);
                    }
                }
            }
            for (int i = 0; i < cape.numberOfPoints - 1; i++)
            {
                DistanceJoint2D joint = cape.capePoints[i].AddComponent<DistanceJoint2D>();
                joint.connectedBody = cape.capePoints[i + 1].GetComponent<Rigidbody2D>();
                cape.capePoints[i + 1].GetComponent<Rigidbody2D>().interpolation = RigidbodyInterpolation2D.Interpolate;
                joint.autoConfigureDistance = false;
                joint.maxDistanceOnly = true;
                joint.distance = cape.pointSpacing * cape.sizeMultiplier;
            }
        }

        public void Update()
        {
            lineRenderer.material.mainTexture = capeTexture;

            Vector3[] positions = new Vector3[(numberOfPoints - 1) * 10 + 1];

            for (int i = 0; i < numberOfPoints - 1; i++)
            {
                Vector3 p0 = (i > 0) ? capePoints[i - 1].transform.localPosition : capePoints[i].transform.localPosition;
                Vector3 p1 = capePoints[i].transform.localPosition;
                Vector3 p2 = capePoints[i + 1].transform.localPosition;
                Vector3 p3 = (i < numberOfPoints - 2) ? capePoints[i + 2].transform.localPosition : capePoints[i + 1].transform.localPosition;

                for (int j = 0; j < 10; j++)
                {
                    float t = j / 10f;
                    positions[i * 10 + j] = 0.5f * ((2 * p1) + (-p0 + p2) * t + (2 * p0 - 5 * p1 + 4 * p2 - p3) * t * t + (-p0 + 3 * p1 - 3 * p2 + p3) * t * t * t);
                }
            }

            positions[positions.Length - 1] = capePoints[numberOfPoints - 1].transform.localPosition;

            lineRenderer.positionCount = positions.Length;
            lineRenderer.SetPositions(positions);

            if (transform.root.localScale.x < 0)
            {
                lineRenderer.widthMultiplier = -1f;
            }
            else
            {
                lineRenderer.widthMultiplier = 1f;
            }
        }
    }

    public class PumpkinLauncher : MonoBehaviour, Mod.ModAPIPlus.IUse2
    {
        public Vector2 barrelPosition;

        public Vector2 barrelDirection;

        private Rigidbody2D rb;

        private PhysicalBehaviour phys;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            phys = GetComponent<PhysicalBehaviour>();
        }

        public void Use2()
        {
            if (base.enabled)
            {
                Launch();
            }
        }

        public void Launch()
        {
            StartCoroutine(LaunchRoutine());
        }

        private IEnumerator LaunchRoutine()
        {
            yield return new WaitForFixedUpdate();
            Quaternion rotation = ((base.transform.lossyScale.x < 0f) ? (Quaternion.Euler(0f, 0f, 180f) * base.transform.rotation) : base.transform.rotation);
            GameObject gameObject;
            PhysicalBehaviour component;
            gameObject = UnityEngine.Object.Instantiate(ModAPI.FindSpawnable("[Nova's Spider-Man Mod] Pumpkin Bomb").Prefab, GetBarrelPosition(), rotation);
            CatalogBehaviour.PerformMod(ModAPI.FindSpawnable("[Nova's Spider-Man Mod] Pumpkin Bomb"), gameObject);

            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);

            gameObject.GetComponent<StickyGrenadeBehaviour>().Explosive.Activate();
            component = gameObject.GetComponent<PhysicalBehaviour>();

            if ((bool)component)
            {
                if (component.SimulateTemperature && (bool)phys && phys.SimulateTemperature)
                {
                    component.Temperature = phys.Temperature;
                }

                component.rigidbody.AddForce(GetBarrelDirection() * 0.3f, ForceMode2D.Impulse);
            }
        }

        public Vector2 GetBarrelPosition()
        {
            return base.transform.TransformPoint(barrelPosition);
        }

        public Vector2 GetBarrelDirection()
        {
            return base.transform.TransformDirection(barrelDirection) * base.transform.localScale.x;
        }
    }

    public class Glider : MonoBehaviour, Messages.IUse
    {
        public bool InFlight = false;

        public PersonBehaviour person;
        float ogbasestrength = 8.5f;
        public DistanceJoint2D springJoint;
        public DistanceJoint2D springJoint2;
        GameObject SpriteLightFlash;
        public void Start()
        {
            SpriteLightFlash = Instantiate(ModAPI.FindSpawnable("Mini Thruster").Prefab.gameObject);
            foreach (Component comp in SpriteLightFlash.GetComponents<MonoBehaviour>())
            {
                if (comp.GetType() != typeof(ParticleSystem) || comp.GetType() != typeof(ParticleSystemRenderer) || comp.GetType() != typeof(Transform) || comp.GetType() != typeof(GameObject))
                {
                    Destroy(comp);
                }
            }
            Destroy(SpriteLightFlash.GetComponent<GlowingHotMetalBehaviour>());
            Destroy(SpriteLightFlash.GetComponent<PhysicalBehaviour>());
            Destroy(SpriteLightFlash.GetComponent<Rigidbody2D>());
            Destroy(SpriteLightFlash.GetComponent<BoxCollider2D>());
            Destroy(SpriteLightFlash.GetComponent<AudioSource>());
            Destroy(SpriteLightFlash.GetComponent<AudioSource>());
            Destroy(SpriteLightFlash.GetComponent<SpriteRenderer>());
            SpriteLightFlash.transform.parent = transform;
            SpriteLightFlash.transform.localPosition = new Vector3(-0.5f, 0.08f, 0);

            SpriteLightFlash.transform.localScale = SpriteLightFlash.transform.localScale.x<0?new Vector3(-0.5f, 0.3f, 0.4f): new Vector3(0.5f, 0.3f, 0.4f);

        }

        public void OnCollisionEnter2D(Collision2D col)
        {
            if (person == null && InFlight)
            {
                if (col.gameObject.TryGetComponent<LimbBehaviour>(out var limb) && col.gameObject.name.Contains("Foot"))
                {
                    person = limb.Person;
                    springJoint = person.Limbs[6].gameObject.AddComponent<DistanceJoint2D>();
                    springJoint.connectedBody = gameObject.GetComponent<Rigidbody2D>();
                    springJoint.autoConfigureConnectedAnchor = false;
                    springJoint.autoConfigureDistance = false;
                    springJoint.distance = 0;
                    springJoint.connectedAnchor = new Vector2(0.3f, 0.2f);

                    springJoint2 = person.Limbs[9].gameObject.AddComponent<DistanceJoint2D>();
                    springJoint2.connectedBody = gameObject.GetComponent<Rigidbody2D>();
                    springJoint2.autoConfigureConnectedAnchor = false;
                    springJoint2.autoConfigureDistance = false;
                    springJoint2.distance = 0;
                    springJoint2.connectedAnchor = new Vector2(-0.3f, 0.2f);
                    //69 heh.. its funny..

                }
            }
        }

        public void FixedUpdate()
        {
            if (InFlight)
            {
                if (person != null)
                {
                    if (!person.IsAlive())
                        StopFlying();
                }

                GetComponent<Rigidbody2D>().mass = 1.5f;
                GetComponent<Rigidbody2D>().gravityScale = 0;
                GetComponent<Rigidbody2D>().angularVelocity *= 0.98f;
                GetComponent<Rigidbody2D>().velocity *= 0.95f;
                float num4 = gameObject.GetComponent<PhysicalBehaviour>().rigidbody.mass / 1.5f;
                float num5 = 3 * Mathf.Clamp(gameObject.GetComponent<PhysicalBehaviour>().Charge, 1f, 5f) * num4 * num4;
                gameObject.GetComponent<PhysicalBehaviour>().rigidbody.angularVelocity *= Mathf.Pow(0.5f, 1f);
                gameObject.GetComponent<Rigidbody2D>().AddTorque(Mathf.DeltaAngle(transform.eulerAngles.z, 0f) * num5);
                if (person != null)
                {
                    person.OverridePoseIndex = 8;
                    foreach (var limb in person.Limbs)
                    {
                        Physics2D.IgnoreCollision(limb.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);
                        if (limb.name.Contains("Arm") || limb.name.Contains("Leg") || limb.name.Contains("Foot"))
                        {
                            limb.BaseStrength = 0;
                        }
                        limb.GetComponent<Rigidbody2D>().gravityScale = 0;

                        if (limb.GetComponent<Rigidbody2D>().bodyType != RigidbodyType2D.Static)
                        {
                            limb.GetComponent<Rigidbody2D>().angularVelocity *= 0.98f;
                            limb.GetComponent<Rigidbody2D>().velocity *= 0.93f;
                            if (limb.name.Contains("Body") || limb.name.Contains("Foot"))
                            {
                                float num2 = limb.gameObject.GetComponent<PhysicalBehaviour>().rigidbody.mass / 1.5f;
                                float num3 = 15 * Mathf.Clamp(limb.gameObject.GetComponent<PhysicalBehaviour>().Charge, 1f, 5f) * num2 * num2;
                                limb.gameObject.GetComponent<PhysicalBehaviour>().rigidbody.angularVelocity *= Mathf.Pow(0.5f, 1f);
                                limb.gameObject.GetComponent<Rigidbody2D>().AddTorque(Mathf.DeltaAngle(limb.transform.eulerAngles.z, 0f) * num3);
                            }
                        }
                    }
                }
            }
        }

        public void StopFlying()
        {
            InFlight = false;
            SpriteLightFlash.GetComponent<ParticleSystem>().Stop();
            GetComponent<Rigidbody2D>().gravityScale = GetComponent<PhysicalBehaviour>().InitialGravityScale;
            if (person != null)
            {
                person.OverridePoseIndex = -1;
                if (person.TryGetComponent<SuperMass>(out var mass))
                    mass.EnablePower();
                foreach (var limb in person.Limbs)
                {
                    Physics2D.IgnoreCollision(limb.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
                    if (limb.name.Contains("Arm") || limb.name.Contains("Leg") || limb.name.Contains("Foot"))
                    {
                        limb.BaseStrength = ogbasestrength;
                    }
                    limb.GetComponent<Rigidbody2D>().gravityScale = limb.PhysicalBehaviour.InitialGravityScale;
                }


                person = null;
                Destroy(springJoint);
                Destroy(springJoint2);
            }
        }

        public void StartFlying()
        {
            if (!InFlight)
            {
                SpriteLightFlash.GetComponent<ParticleSystem>().Play();
                InFlight = true;
                if (person != null)
                {
                    person.OverridePoseIndex = 8;
                    if(person.TryGetComponent<SuperMass>(out var mass))
                        mass.DisablePower();
                    foreach (var limb in person.Limbs)
                    {
                        if (limb.name.Contains("Arm") || limb.name.Contains("Leg") || limb.name.Contains("Foot"))
                        {
                            limb.BaseStrength = 0;
                        }
                        limb.GetComponent<Rigidbody2D>().gravityScale = 0;
                    }
                }
            }
        }

        public void Use(ActivationPropagation activation)
        {
            if (!InFlight)
            {
                StartFlying();
            }
            else
            {
                StopFlying();
            }
        }

    }

    public class AlternateMouseActivator : MonoBehaviour
    {
        public void Update()
        {
            if (InputSystem.Down("NovaKeyActivation"))
            {
                HashSet<GameObject> activatedThirdChildren = new HashSet<GameObject>();

                if (SelectionController.Main.CurrentlyUnderMouse != null)
                {
                    var underMouse = SelectionController.Main.CurrentlyUnderMouse;
                    if (underMouse.TryGetComponent<LimbBehaviour>(out var limb) && limb.name.Contains("LowerLeg"))
                    {
                        Transform thirdChild = underMouse.transform.parent.GetChild(2);
                        if (activatedThirdChildren.Add(thirdChild.gameObject) && !SelectionController.Main.SelectedObjects.Contains(thirdChild.GetComponent<PhysicalBehaviour>()))
                        {
                            foreach (var comp in thirdChild.GetComponents<MonoBehaviour>())
                            {
                                if (comp is Mod.ModAPIPlus.IUse2 us)
                                {
                                    us.Use2();
                                }
                            }
                        }
                    }

                    foreach (var comp in underMouse.GetComponents<MonoBehaviour>())
                    {
                        if (comp is Mod.ModAPIPlus.IUse2 us &&
                            !(underMouse.TryGetComponent<LimbBehaviour>(out var lb) && lb.name.Contains("LowerLeg")))
                        {
                            us.Use2();
                        }
                    }
                }

                foreach (var hit in SelectionController.Main.SelectedObjects)
                {
                    if (hit.TryGetComponent<LimbBehaviour>(out var limb) && limb.name.Contains("LowerLeg"))
                    {
                        Transform thirdChild = hit.transform.parent.GetChild(2);
                        if (activatedThirdChildren.Add(thirdChild.gameObject) && !SelectionController.Main.SelectedObjects.Contains(thirdChild.GetComponent<PhysicalBehaviour>()))
                        {
                            foreach (var comp in thirdChild.GetComponents<MonoBehaviour>())
                            {
                                if (comp is Mod.ModAPIPlus.IUse2 us)
                                {
                                    us.Use2();
                                }
                            }
                        }
                    }

                    foreach (var comp in hit.GetComponents<MonoBehaviour>())
                    {
                        if (comp is Mod.ModAPIPlus.IUse2 us &&
                            !(hit.TryGetComponent<LimbBehaviour>(out var lb) && lb.name.Contains("LowerLeg")) && hit != SelectionController.Main.CurrentlyUnderMouse)
                        {
                            us.Use2();
                        }
                    }
                }
            }
        }
    }

    public class Teleport : Power, Messages.IUse
    {
        public Teleportation teleportation;

        public static Power SetPower(PersonBehaviour Person, LimbBehaviour Limb, Sprite icon)
        {
            var power = Limb.gameObject.AddComponent<Teleport>();
            power.Name = "Teleportation";
            power.Description = "Activate the user's upper body to allow them to teleport to the location of your mouse cursor when left click is pressed";
            power.icon = icon;

            power.targetLimb = TargettedLimb.Body;
            var teleportthing = new GameObject("teleportHandler" + UnityEngine.Random.Range(0, 200000));
            teleportthing.AddComponent<Teleportation>().person = Person;

            power.teleportation = teleportthing.GetComponent<Teleportation>();
            return power;
        }

        public override void Start()
        {
            foreach (var limb in GetComponent<LimbBehaviour>().Person.Limbs)
            {
                if (limb.HasJoint)
                {
                    limb.Joint.autoConfigureConnectedAnchor = false;
                }
            }
            base.Start();
        }

        public void Use(ActivationPropagation activation)
        {
            if (Enabled)
                teleportation.Use();
        }

        public class Teleportation : MonoBehaviour
        {
            public bool teleporting;
            public PersonBehaviour person;
            public GameObject Smoke;

            public void Start()
            {
                Smoke = Instantiate(ModAPI.FindSpawnable("Particle Projector").Prefab.transform.GetChild(0).gameObject);
                var pfx = Smoke.GetComponent<ParticleSystem>();
                pfx.playOnAwake = false;
                pfx.Stop();
                pfx.gravityModifier = 0;
                pfx.startSpeed = 0;
                pfx.startColor = new Color(0, 0.28f, 0, 0.102f);
            }

            public void Use()
            {
                ModAPI.CreateParticleEffect("Flash", person.Limbs[2].transform.position);
                Smoke.GetComponent<ParticleSystem>().Play();
                person.gameObject.SetActive(false);

                var limbStatusObjects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "Limb status(Clone)");

                foreach (var status in limbStatusObjects)
                {
                    foreach (var limb in person.Limbs)
                    {
                        if (status.GetComponent<LimbStatusBehaviour>().limb == limb)
                        {
                            status.GetComponent<LimbStatusBehaviour>().enabled = false;
                            var color = status.GetComponent<SpriteRenderer>().color;
                            status.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 0);
                            var color2 = status.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
                            status.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(color2.r, color2.g, color2.b, 0);
                        }
                    }
                }

                teleporting = true;
            }

            public void Update()
            {
                Vector3 mouseScreenPosition = Input.mousePosition;
                Smoke.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, Camera.main.nearClipPlane));

                if (Input.GetKey(KeyCode.Mouse0) && teleporting)
                {
                    teleporting = false;
                    Smoke.GetComponent<ParticleSystem>().Stop();
                    CameraShakeBehaviour.main.Shake(1, person.Limbs[0].transform.position);
                    Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, Camera.main.nearClipPlane));

                    foreach (var limb in person.Limbs)
                    {
                        Vector3 newPosition = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, limb.transform.position.z);
                        limb.transform.position = newPosition;
                    }
                    person.gameObject.SetActive(true);
                    ModAPI.CreateParticleEffect("Flash", person.Limbs[2].transform.position).transform.localScale = new Vector3(2, 2, 2);
                    foreach (var limb in person.Limbs)
                    {
                        foreach (var limb2 in person.Limbs)
                        {
                            foreach (AudioSource aud in limb.GetComponents<AudioSource>())
                                aud.Stop();
                            Physics2D.IgnoreCollision(limb.GetComponent<Collider2D>(), limb2.GetComponent<Collider2D>());
                        }
                    }

                    var limbStatusObjects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "Limb status(Clone)");

                    foreach (var status in limbStatusObjects)
                    {
                        foreach (var limb in person.Limbs)
                        {
                            if (status.GetComponent<LimbStatusBehaviour>().limb == limb)
                            {
                                status.GetComponent<LimbStatusBehaviour>().enabled = true;
                                var color = status.GetComponent<SpriteRenderer>().color;
                                status.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 1);
                                var color2 = status.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
                                status.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(color2.r, color2.g, color2.b, 1);
                            }
                        }
                    }
                }
            }

        }
    }

    public class WebWireFixer : MonoBehaviour
    {
        public float OGDistance;
        public float OGStrength;
        public SpringJoint2D springJoint;

        public void Start()
        {
            OGDistance = springJoint ? springJoint.distance:0;
            OGStrength = springJoint ? springJoint.frequency : 1;
        }

        public void Update()
        {
            if (!springJoint)
                return;

            float currentDistance = Vector2.Distance(transform.position, springJoint.connectedAnchor);

            if(springJoint.connectedBody)
                currentDistance = Vector2.Distance(transform.position, springJoint.connectedBody.transform.TransformPoint(springJoint.connectedAnchor));

            if (currentDistance < OGDistance)
            {
                springJoint.frequency = 0.0001f;
            }else
                springJoint.frequency = OGStrength;
        }
    }

    public class FakeNotifDissapearer : MonoBehaviour
    {
        private float fadeSpeed = 0.8f;
        private float lastAlpha = 1f;

        public void Update()
        {
            var sr = GetComponent<SpriteRenderer>();
            Color targetColor = new Color(1, 1, 1, 0);
            if (sr.color.a > 0.01f)
            {
                // Use unscaledDeltaTime to ignore timescale
                sr.color = Color.Lerp(sr.color, targetColor, fadeSpeed * Time.unscaledDeltaTime * 10f);
                lastAlpha = sr.color.a;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    [SkipSerialisation]
    public class BreakWebDecal : MonoBehaviour
    {
        public GameObject webdecal;
        public Joint2D joint;

        public void Update()
        {
            if (joint == null)
            {
                Destroy(webdecal);
                Destroy(this);
            }
        }

        public void OnJointBreak2D(Joint2D jointt)
        {
            if (jointt == joint)
            {
                Destroy(webdecal);
                Destroy(this);
            } 
        }
    }

    public class WebShot : MonoBehaviour
    {
        public PersonBehaviour person;
        private bool hasCollided = false;

        void JointCreated(SpringJoint2D joint)
        {
            var energyWire = joint.gameObject.AddComponent<WebWireBehaviour>();
            energyWire.WireColor = new Color(205, 205, 207);
            energyWire.WireMaterial = Instantiate(ModAPI.FindSpawnable("Brick").Prefab.GetComponent<SpriteRenderer>().material);
            energyWire.WireWidth = 0.055f;
            energyWire.typedJoint = joint;
            energyWire.WireMaterial.mainTexture = Mod.web.texture;
            energyWire.WireMaterial.mainTexture.wrapMode = TextureWrapMode.Repeat;
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (hasCollided) return;
            hasCollided = true;

            if (collision.gameObject.TryGetComponent<LimbBehaviour>(out var hitPerson))
            {
                if (hitPerson.Person != person)
                {
                    StartCoroutine(PlaySound(Mod.WebSFX[UnityEngine.Random.Range(0, Mod.WebSFX.Length)]));
                    var grappleJoint = collision.gameObject.AddComponent<SpringJoint2D>();
                    grappleJoint.anchor = new Vector2(UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f));
                    grappleJoint.autoConfigureDistance = false;
                    grappleJoint.dampingRatio = 0.9f;
                    grappleJoint.frequency = 3f;
                    grappleJoint.enableCollision = true;
                    grappleJoint.connectedAnchor = collision.contacts[0].point;
                    grappleJoint.distance = 0;
                    grappleJoint.breakForce = 1000 * collision.rigidbody.mass;
                    JointCreated(grappleJoint);

                    var webDecal = new GameObject("Web Decal");
                    webDecal.transform.position = collision.contacts[0].point;
                    webDecal.transform.rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360));
                    webDecal.transform.localScale = Vector3.one * UnityEngine.Random.Range(0.05f, 0.5f);
                    webDecal.AddComponent<DebrisComponent>();
                    var sr = webDecal.AddComponent<SpriteRenderer>();
                    sr.sprite = Mod.WebAnchor;
                    sr.sortingLayerName = "Bottom";
                    sr.sortingOrder = -1000;
                    sr.color = new Color(1, 1, 1, 0.4f);
                    var breakweb = collision.gameObject.AddComponent<BreakWebDecal>();
                    breakweb.webdecal = webDecal;
                    breakweb.joint = grappleJoint;
                    Destroy(gameObject);
                }else
                {
                    hasCollided = false;
                }
            }
            else if (!collision.gameObject.GetComponent<LimbBehaviour>() && !collision.gameObject.GetComponent<WebShot>())
            {
                if (collision.collider.gameObject.layer != 9)
                {
                    StartCoroutine(PlaySound(Mod.WebSFX[UnityEngine.Random.Range(0, Mod.WebSFX.Length)]));
                    Destroy(gameObject);
                }
                else
                {
                    StartCoroutine(PlaySound(Mod.WebSFX[UnityEngine.Random.Range(0, Mod.WebSFX.Length)]));
                    var grappleJoint = collision.gameObject.AddComponent<SpringJoint2D>();
                    grappleJoint.anchor = new Vector2(UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f));
                    grappleJoint.autoConfigureDistance = false;
                    grappleJoint.dampingRatio = 0.9f;
                    grappleJoint.frequency = 3f;
                    grappleJoint.enableCollision = true;
                    grappleJoint.connectedAnchor = collision.contacts[0].point;
                    grappleJoint.distance = 0;
                    grappleJoint.breakForce = 1000 * collision.rigidbody.mass;
                    JointCreated(grappleJoint);

                    var webDecal = new GameObject("Web Decal");
                    webDecal.transform.position = collision.contacts[0].point;
                    webDecal.transform.rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360));
                    webDecal.transform.localScale = Vector3.one * UnityEngine.Random.Range(0.05f, 0.5f);
                    webDecal.layer = 10;
                    webDecal.AddComponent<DebrisComponent>();
                    var sr = webDecal.AddComponent<SpriteRenderer>();
                    sr.sprite = Mod.WebAnchor;
                    sr.sortingLayerName = "Bottom";
                    sr.sortingOrder = -1000;
                    sr.color = new Color(1, 1, 1, 0.4f);
                    var breakweb = collision.gameObject.AddComponent<BreakWebDecal>();
                    breakweb.webdecal = webDecal;
                    breakweb.joint = grappleJoint;
                    Destroy(gameObject);
                }
            }
            else
            {
                hasCollided = false;
            }
        }

        private IEnumerator PlaySound(AudioClip SoundToPlay)
        {
            var sound = new GameObject();
            sound.name = "SFX";
            sound.transform.position = gameObject.transform.position;
            sound.AddComponent<AudioSource>();
            sound.GetComponent<AudioSource>().playOnAwake = false;
            sound.GetComponent<AudioSource>().clip = SoundToPlay;
            sound.GetComponent<AudioSource>().spatialBlend = 1;
            sound.GetComponent<AudioSource>().volume = 5f;
            sound.AddComponent<AudioDistortionFilter>().distortionLevel = 0.85f;
            sound.AddComponent<AudioSourceTimeScaleBehaviour>();
            sound.GetComponent<AudioSource>().Play();
            sound.AddComponent<OBJDestroyer>();
            yield return new WaitForSeconds(SoundToPlay.length);

            Destroy(sound);
        }
    }

    public enum WebType
    {
        Normal,
        Connector,
        Grapple,
        Electric,
        Webshot,
        None
    }

    public class OBJDestroyer : MonoBehaviour
    {
        public void Start()
        {
            StartCoroutine(DestroyAfterTime(60f));
        }

        IEnumerator DestroyAfterTime(float time)
        {
            yield return new WaitForSeconds(time);
            Destroy(gameObject);
        }
    }

    public class WebSlinging : Power, Messages.IUse, Mod.ModAPIPlus.IUse2
    {
        public bool Rigid = false;
        private SpringJoint2D grappleJoint;
        private DistanceJoint2D rigidGrappleJoint;
        private Rigidbody2D rb;
        private Vector2 anchorPoint;
        private float OGDistance;
        public float OGStrength;
        WebType webType = WebType.Normal;
        RaycastHit2D hit;

        public void Use2()
        {
            int nextType = ((int)webType + 1) % Enum.GetValues(typeof(WebType)).Length;
            webType = (WebType)nextType;
            var text = new GameObject("WebTypeText");
            var sr = text.AddComponent<SpriteRenderer>();
            sr.sortingLayerName = "Top";
            sr.sortingOrder = 1000;
            text.transform.position = transform.position;
            text.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            text.AddComponent<FakeNotifDissapearer>();
            if (webType == WebType.Normal)
                sr.sprite = Mod.Normal;
            else if (webType == WebType.Connector)
                sr.sprite = Mod.Connector;
            else if (webType == WebType.Grapple)
                sr.sprite = Mod.Grapple;
            else if (webType == WebType.Electric)
                sr.sprite = Mod.Electric;
            else if (webType == WebType.Webshot)
                sr.sprite = Mod.Webshot;
            else if (webType == WebType.None)
                sr.sprite = Mod.None;
        }

        public static Power SetPower(PersonBehaviour Person, LimbBehaviour Limb, Sprite icon)
        {
            var power = Limb.gameObject.AddComponent<WebSlinging>();
            power.Name = "Webslinging";
            power.Description = "Allows the user to fire webs with the activation key.<color=\"yellow\">\nRight click to toggle web rigidity\nUse alternate activation key (typically H) to toggle web type\n Web Types: Normal, Connector, Grapple, Electric, Webshot, None";
            power.icon = icon;
            if (power.name.Contains("Front"))
            {
                power.targetLimb = TargettedLimb.FrontArm;
            }
            else
            {
                power.targetLimb = TargettedLimb.BackArm;
            }
            Limb.PhysicalBehaviour.ForceNoCharge = true;
            Limb.gameObject.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton(() => !ColorpickerDialogBehaviour.IsOpen, "ToggleRigidity", "Toggle Web Rigidity", "Toggle Web Rigidity", delegate
            {
                power.Rigid = !power.Rigid;
            }));

            return power;
        }

        public void OnDestroy()
        {
            Destroy(grappleJoint);
            Destroy(rigidGrappleJoint);
        }

        public override void DisablePower()
        {
            base.DisablePower();
            foreach (var wire in GetComponents<WebWireBehaviour>())
            {
                Destroy(wire);
            }
            foreach (var wire in GetComponents<EnergyWireBehaviour>())
            {
                Destroy(wire);
            }
            Destroy(grappleJoint);
            Destroy(rigidGrappleJoint);
        }

        public void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void JointCreated(SpringJoint2D joint)
        {
            var energyWire = joint.gameObject.AddComponent<WebWireBehaviour>();
            energyWire.WireColor = new Color(205, 205, 207);
            energyWire.WireMaterial = Instantiate(ModAPI.FindSpawnable("Brick").Prefab.GetComponent<SpriteRenderer>().material);
            energyWire.WireWidth = 0.055f;
            energyWire.typedJoint = joint;
            energyWire.WireMaterial.mainTexture = webType==WebType.Electric?Mod.electricWeb.texture:Mod.web.texture;
            energyWire.WireMaterial.mainTexture.wrapMode = TextureWrapMode.Repeat;
        }

        void JointCreated(DistanceJoint2D joint)
        {
            var energyWire = joint.gameObject.AddComponent<EnergyWireBehaviour>();
            energyWire.WireColor = new Color(205, 205, 207);
            energyWire.WireMaterial = Instantiate(ModAPI.FindSpawnable("Brick").Prefab.GetComponent<SpriteRenderer>().material);
            energyWire.WireWidth = 0.055f;
            energyWire.typedJoint = joint;
            energyWire.WireMaterial.mainTexture = webType == WebType.Electric ? Mod.electricWeb.texture : Mod.web.texture;
            energyWire.WireMaterial.mainTexture.wrapMode = TextureWrapMode.Repeat;
        }

        public void Use(ActivationPropagation activation)
        {
            if (webType == WebType.Webshot)
            {
                var webshot = ModAPI.CreatePhysicalObject("Webshot", Mod.WebShot);
                webshot.GetComponent<PhysicalBehaviour>().Properties = ModAPI.FindPhysicalProperties("Soap");
                webshot.transform.position = transform.position + transform.up * -0.5f;
                webshot.GetComponent<Rigidbody2D>().velocity = transform.up * -10f;
                webshot.GetComponent<Rigidbody2D>().angularVelocity = UnityEngine.Random.Range(-25, 25);
                webshot.AddComponent<WebShot>();
                webshot.GetComponent<WebShot>().person = GetComponent<LimbBehaviour>().Person;
                Debug.Log(GetComponent<LimbBehaviour>().Person.name);
                webshot.AddComponent<TrailRenderer>().startColor = Color.white;
                webshot.GetComponent<TrailRenderer>().endColor = Color.white;
                webshot.GetComponent<TrailRenderer>().startWidth = 0.05f;
                webshot.GetComponent<TrailRenderer>().endWidth = 0.01f;
                webshot.GetComponent<TrailRenderer>().time = 0.2f;
                webshot.GetComponent<TrailRenderer>().material = ModAPI.FindSpawnable("Brick").Prefab.GetComponent<SpriteRenderer>().material;
            }

            if (grappleJoint != null || rigidGrappleJoint != null)
            {
                if (webType == WebType.Connector)
                {
                    Vector2 origin = transform.position + transform.up * -0.1f;
                    Vector2 direction = -transform.up;
                    float maxDistance = 25f;

                    RaycastHit2D hit = Physics2D.Raycast(origin, direction, maxDistance);
                    RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, maxDistance);
                    bool ishit = true;

                    foreach (var hitt in hits)
                    {
                        if (hitt.collider.TryGetComponent<LimbBehaviour>(out var limb))
                        {
                            if (limb.Person != GetComponent<LimbBehaviour>().Person)
                            {
                                hit = hitt; break;
                            }
                        }else
                        {
                            hit = hitt; break;
                        }
                    }

                    if (hits == null)
                    {
                        ishit = false;
                    }

                    if (hit.collider != null && hit.rigidbody != null && !hit.collider.name.Contains("Cape") && ishit == true)
                    {
                        StartCoroutine(PlaySound(Mod.WebSFX[UnityEngine.Random.Range(0, Mod.WebSFX.Length)]));
                        var connected = grappleJoint != null ? grappleJoint.connectedBody.gameObject : rigidGrappleJoint.connectedBody.gameObject;
                        if (Rigid)
                        {
                            var newrigidjoint = connected.AddComponent<DistanceJoint2D>();
                            newrigidjoint.anchor = grappleJoint != null ? grappleJoint.connectedAnchor : rigidGrappleJoint.connectedAnchor;
                            newrigidjoint.connectedBody = hit.rigidbody;
                            newrigidjoint.autoConfigureDistance = false;
                            newrigidjoint.enableCollision = true;
                            newrigidjoint.connectedAnchor = hit.rigidbody.transform.InverseTransformPoint(hit.point);
                            newrigidjoint.distance = Vector2.Distance(connected.transform.position, hit.point);
                            newrigidjoint.maxDistanceOnly = true;
                            newrigidjoint.breakForce = 5000 * hit.rigidbody.mass;
                            JointCreated(newrigidjoint);
                        }
                        else
                        {
                            var newjoint = connected.AddComponent<SpringJoint2D>();
                            connected.AddComponent<WebWireFixer>().springJoint = newjoint;
                            newjoint.anchor = grappleJoint != null ? grappleJoint.connectedAnchor : rigidGrappleJoint.connectedAnchor;
                            newjoint.connectedBody = hit.rigidbody;
                            newjoint.autoConfigureDistance = false;
                            newjoint.distance = Vector2.Distance(connected.transform.position, hit.point);
                            newjoint.dampingRatio = 0.9f;
                            newjoint.frequency = 4f;
                            newjoint.enableCollision = true;
                            newjoint.connectedAnchor = hit.rigidbody.transform.InverseTransformPoint(hit.point);
                            newjoint.breakForce = 2500 * hit.rigidbody.mass;
                            JointCreated(newjoint);
                        }
                    }
                }

                Destroy(grappleJoint);
                Destroy(rigidGrappleJoint);
                foreach (var wire in GetComponents<WebWireBehaviour>())
                {
                    Destroy(wire);
                }
                foreach (var wire in GetComponents<EnergyWireBehaviour>())
                {
                    Destroy(wire);
                }
                StartCoroutine(PlaySound(Mod.WebSFX[UnityEngine.Random.Range(0, Mod.WebSFX.Length)]));
            }
            else if(grappleJoint == null && rigidGrappleJoint == null && Enabled)
            {
                if (webType == WebType.None || webType == WebType.Webshot)
                    return;
                Vector2 origin = transform.position + transform.up * -0.1f;
                Vector2 direction = -transform.up;
                float maxDistance = 25f;

                RaycastHit2D hit = Physics2D.Raycast(origin, direction, maxDistance);
                RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, maxDistance);
                bool ishit = true;

                foreach (var hitt in hits)
                {
                    if (hitt.collider.TryGetComponent<LimbBehaviour>(out var limb))
                    {
                        if (limb.Person != GetComponent<LimbBehaviour>().Person)
                        {
                            hit = hitt; break;
                        }
                    }
                    else
                    {
                        if(hitt.collider.GetComponent<PhysicalBehaviour>())
                            hit = hitt; break;
                    }
                }

                if (hits == null)
                {
                    ishit = false;
                }

                if (hit.collider != null)
                {
                    if (hit.collider.TryGetComponent<LimbBehaviour>(out var limb))
                    {
                        if(limb.Person == GetComponent<LimbBehaviour>().Person)
                            ishit = false;
                    }
                }

                if (hit.collider != null && hit.rigidbody != null && ishit == true)
                {
                    if (hit.collider.TryGetComponent<LimbBehaviour>(out var lim))
                        if(lim.Person == GetComponent<LimbBehaviour>().Person)
                            return;
                    if (hit.collider.gameObject.layer != 9 && webType == WebType.Connector)
                        return;

                    StartCoroutine(PlaySound(Mod.WebSFX[UnityEngine.Random.Range(0, Mod.WebSFX.Length)]));

                    if (Rigid)
                    {
                        rigidGrappleJoint = gameObject.AddComponent<DistanceJoint2D>();
                        rigidGrappleJoint.anchor = new Vector2(0, -0.25f);
                        rigidGrappleJoint.connectedBody = hit.rigidbody;
                        rigidGrappleJoint.autoConfigureDistance = false;
                        rigidGrappleJoint.enableCollision = true;
                        rigidGrappleJoint.connectedAnchor = hit.rigidbody.transform.InverseTransformPoint(hit.point);
                        rigidGrappleJoint.distance = Vector2.Distance(transform.position, hit.point);
                        rigidGrappleJoint.maxDistanceOnly = true;
                        rigidGrappleJoint.breakForce = 5000 * hit.rigidbody.mass;
                        OGDistance = rigidGrappleJoint.distance;
                        JointCreated(rigidGrappleJoint);
                    }
                    else
                    {
                        grappleJoint = gameObject.AddComponent<SpringJoint2D>();
                        grappleJoint.anchor = new Vector2(0, -0.25f);
                        grappleJoint.connectedBody = hit.rigidbody;
                        grappleJoint.autoConfigureDistance = false;
                        grappleJoint.distance = Vector2.Distance(transform.position, hit.point);
                        grappleJoint.dampingRatio = 0.9f;
                        grappleJoint.frequency = 4f;
                        grappleJoint.enableCollision = true;
                        grappleJoint.connectedAnchor = hit.rigidbody.transform.InverseTransformPoint(hit.point);
                        grappleJoint.distance = Vector2.Distance(transform.position, hit.point);
                        OGDistance = grappleJoint.distance;
                        OGStrength = grappleJoint.frequency;
                        JointCreated(grappleJoint);
                    }
                }
                else
                {
                    if (webType == WebType.Connector)
                        return;

                    StartCoroutine(PlaySound(Mod.WebSFX[UnityEngine.Random.Range(0, Mod.WebSFX.Length)]));

                    if (Rigid)
                    {
                        rigidGrappleJoint = gameObject.AddComponent<DistanceJoint2D>();
                        rigidGrappleJoint.anchor = new Vector2(0, -0.25f);
                        rigidGrappleJoint.connectedAnchor = origin + direction * 10f;
                        rigidGrappleJoint.autoConfigureDistance = false;
                        rigidGrappleJoint.enableCollision = true;
                        rigidGrappleJoint.distance = Vector2.Distance(transform.position, origin + direction * 10f);
                        rigidGrappleJoint.maxDistanceOnly = true;
                        rigidGrappleJoint.breakForce = 500;
                        OGDistance = rigidGrappleJoint.distance;
                        JointCreated(rigidGrappleJoint);
                    }
                    else
                    {
                        grappleJoint = gameObject.AddComponent<SpringJoint2D>();
                        grappleJoint.anchor = new Vector2(0, -0.25f);
                        grappleJoint.connectedAnchor = origin + direction * 10f;
                        grappleJoint.autoConfigureDistance = false;
                        grappleJoint.dampingRatio = 0.9f;
                        grappleJoint.frequency = 4f;
                        grappleJoint.enableCollision = true;
                        grappleJoint.distance = Vector2.Distance(transform.position, origin + direction * 10f);
                        OGDistance = grappleJoint.distance;
                        OGStrength = grappleJoint.frequency;
                        JointCreated(grappleJoint);
                    }
                }
            }
        }
        bool didstretchsound = false;

        public IEnumerator stretch()
        {
            didstretchsound = true;
            yield return new WaitForSeconds(UnityEngine.Random.Range(1, 5));
            didstretchsound = false;
        }

        public void Update()
        {
            if (grappleJoint != null)
            {
                float currentDistance = Vector2.Distance(transform.position, grappleJoint.connectedAnchor);

                if(grappleJoint.connectedBody)
                    currentDistance = Vector2.Distance(transform.position, grappleJoint.connectedBody.transform.TransformPoint(grappleJoint.connectedAnchor));

                if (currentDistance < OGDistance)
                {
                    grappleJoint.frequency = 0.0001f;
                }
                else
                    grappleJoint.frequency = OGStrength;

                if (currentDistance > OGDistance * 1.5f || webType == WebType.Grapple)
                {
                    if (!didstretchsound)
                    {
                        StartCoroutine(PlaySound(Mod.WebStretch));
                        StartCoroutine(stretch());
                    }
                }
            }
        }

        public void FixedUpdate()
        {
            if (grappleJoint && webType == WebType.Electric && grappleJoint.connectedAnchor != null)
                grappleJoint.connectedBody.GetComponent<PhysicalBehaviour>().charge += 0.05f;

            if(rigidGrappleJoint && webType == WebType.Electric && grappleJoint.connectedAnchor != null)
                rigidGrappleJoint.connectedBody.GetComponent<PhysicalBehaviour>().charge += 0.05f;

            if (grappleJoint && webType == WebType.Grapple)
            {
                grappleJoint.distance = Mathf.Lerp(grappleJoint.distance, 0, 0.1f);
                OGDistance = grappleJoint.distance;
            }
        }

        private IEnumerator PlaySound(AudioClip SoundToPlay)
        {
            var sound = new GameObject();
            sound.name = "SFX";
            sound.transform.position = gameObject.transform.position;
            sound.AddComponent<AudioSource>();
            sound.GetComponent<AudioSource>().playOnAwake = false;
            sound.GetComponent<AudioSource>().clip = SoundToPlay;
            sound.GetComponent<AudioSource>().spatialBlend = 1;
            sound.GetComponent<AudioSource>().volume = 5f;
            sound.AddComponent<AudioDistortionFilter>().distortionLevel = 0.85f;
            sound.AddComponent<AudioSourceTimeScaleBehaviour>();
            sound.GetComponent<AudioSource>().Play();
            sound.AddComponent<OBJDestroyer>();
            yield return new WaitForSeconds(SoundToPlay.length);

            Destroy(sound);
        }
    }

    public class WebWireBehaviour : SpringJointWireBehaviour
    {

        protected override void Created()
        {
            base.Created();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }

    public abstract class SpringJointWireBehaviour : WireBehaviour
    {
        public float currentDistance;

        [SkipSerialisation]
        public SpringJoint2D typedJoint;

        protected virtual void Start()
        {
            if (!typedJoint)
            {
                typedJoint = base.gameObject.AddComponent<SpringJoint2D>();
                untypedJoint = typedJoint;
                typedJoint.autoConfigureConnectedAnchor = false;
                typedJoint.enableCollision = true;
                typedJoint.autoConfigureDistance = false;
                if ((bool)otherPhysicalBehaviour)
                {
                    typedJoint.connectedBody = otherPhysicalBehaviour.rigidbody;
                }

                if ((bool)typedJoint.connectedBody)
                {
                    typedJoint.connectedAnchor = currentConnectedAnchor;
                }
                else
                {
                    typedJoint.connectedAnchor = base.transform.root.TransformPoint(currentConnectedAnchor);
                }

                typedJoint.anchor = currentAnchor;
                typedJoint.distance = currentDistance;
                if (!float.IsInfinity(currentBreakingForce))
                {
                    typedJoint.breakForce = currentBreakingForce;
                }

                WireMaterial = Resources.Load<Material>("Materials/" + WireMaterialName);
                typedJoint.autoConfigureDistance = true;
            }
            else
            {
                untypedJoint = typedJoint;
                currentAnchor = typedJoint.anchor;
                currentDistance = typedJoint.distance;
                currentBreakingForce = typedJoint.breakForce;
                if ((bool)typedJoint.connectedBody)
                {
                    currentConnectedAnchor = typedJoint.connectedAnchor;
                }
                else
                {
                    currentConnectedAnchor = base.transform.root.InverseTransformPoint(typedJoint.connectedAnchor);
                }

                if (!WireMaterial)
                {
                    WireMaterial = Resources.Load<Material>("Materials/" + WireMaterialName);
                }

                WireMaterialName = (WireMaterial ? WireMaterial.name : WireMaterialName);
            }

            Initialise();
        }

        protected virtual void CalculateParabola(Vector3 pointA, Vector3 pointB, int vertexCount, ref Vector3[] vertices)
        {
            if (vertexCount > 1)
            {
                float num = typedJoint.distance * typedJoint.distance;
                float num2 = Mathf.Pow(1f - (pointA - pointB).sqrMagnitude / num, 0.65f) * typedJoint.distance / 2f;
                for (int i = 0; i < vertexCount; i++)
                {
                    float num3 = (float)i / (float)vertexCount;
                    Vector3 vector = Vector3.Lerp(pointA, pointB, num3);
                    if (num2 > float.Epsilon)
                    {
                        vector.y += ParabolaFunction(num3) * num2;
                    }

                    vertices[i] = vector;
                }
            }

            vertices[0] = pointA;
            vertices[vertexCount] = pointB;
        }

        protected override void Update()
        {
            base.Update();
            if (!shouldIgnoreUpdate)
            {
                Vector3 pointA = base.transform.TransformPoint(untypedJoint.anchor);
                Vector3 pointB = ((!untypedJoint.connectedBody) ? ((Vector3)untypedJoint.connectedAnchor) : untypedJoint.connectedBody.transform.TransformPoint(untypedJoint.connectedAnchor));
                CalculateParabola(pointA, pointB, GetVertexCount(), ref vertices);
                if ((bool)lineChild)
                {
                    lineRenderer.SetPositions(vertices);
                }

                //currentDistance = typedJoint.distance;
            }
        }

        [Obsolete]
        private void CalculateEdgeCollider()
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                points2d[i] = GetComponent<Collider>().transform.InverseTransformPoint(vertices[i]);
            }

            base.EdgeCollider.points = points2d;
            base.EdgeCollider.edgeRadius = WireWidth / 1.5f;
        }

        private float ParabolaFunction(float x)
        {
            return Utils.Cosh(2f * x * 1.316958f - 1.316958f) - 2f;
        }

        public override void OnUserDelete()
        {
        }
    }

    public class OGSprite : MonoBehaviour
    {
        public Sprite OGsprite;

        public void Start()
        {
            if (OGsprite != null)
                return;

            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null && spriteRenderer.sprite != null)
            {
                Texture2D originalTexture = spriteRenderer.sprite.texture;
                Texture2D textureCopy = new Texture2D(originalTexture.width, originalTexture.height, originalTexture.format, originalTexture.mipmapCount > 1);
                textureCopy.SetPixels(originalTexture.GetPixels());
                textureCopy.Apply();

                OGsprite = Sprite.Create(textureCopy, spriteRenderer.sprite.rect, spriteRenderer.sprite.pivot, spriteRenderer.sprite.pixelsPerUnit);
            }
        }
    }

    public class Throwable : MonoBehaviour, Messages.IOnDrop
    {
        private Rigidbody2D rigidbody;
        private float additionalForce = 35f;
        public UnityEvent OnThrow;

        public void Start()
        {
            rigidbody = gameObject.GetComponent<Rigidbody2D>();
            rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

            var phys = gameObject.GetComponent<PhysicalBehaviour>();
            phys.ForceContinuous = true;
        }

        public void Throw(Vector2 direction)
        {
            var mass = rigidbody.mass;
            var force = direction.normalized * mass * additionalForce;
            OnThrow?.Invoke();
            var rotation = Quaternion.LookRotation(Vector3.forward, direction);
            transform.rotation = rotation;
            rigidbody.AddForce(force, ForceMode2D.Impulse);
        }

        public void OnDrop(GripBehaviour Gripper)
        {
            Throw(-Gripper.transform.up);
        }
    }

    public class ShieldBounce : MonoBehaviour, Messages.IOnDrop, Messages.IOnGripped
    {
        private Rigidbody2D rigidbody;
        private float additionalForce = 35f;
        private GripBehaviour targetGrip;
        private int remainingHits = 3; // Number of allowed redirections
        private float activationDistance = 2f; // Distance to activate the Use() method
        Vector2 directionToGrip;
        bool canBounce = true;
        bool canUse = true;
        bool canRicochet = true;

        public IEnumerator WaitForASec()
        {
            canUse = false;
            yield return new WaitForSeconds(0.3f);
            canUse = true;
        }

        public IEnumerator WaitForASecRicochet()
        {
            canRicochet = false;
            yield return new WaitForSeconds(0.3f);
            canRicochet = true;
        }

        public void Start()
        {
            rigidbody = gameObject.GetComponent<Rigidbody2D>();
            rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

            var phys = gameObject.GetComponent<PhysicalBehaviour>();
            phys.ForceContinuous = true;
        }

        public void Throw(Vector2 direction)
        {
            var mass = rigidbody.mass;
            var force = direction.normalized * mass * additionalForce;

            var rotation = Quaternion.LookRotation(Vector3.forward, direction);
            transform.rotation = rotation;
            rigidbody.AddForce(force, ForceMode2D.Impulse);
        }

        public void OnDrop(GripBehaviour Gripper)
        {
            StartCoroutine(WaitForASec());
            targetGrip = Gripper; // Save the grip that dropped the boomerang

            canBounce = true;

            Throw(-Gripper.transform.up);
        }

        public void OnGripped(GripBehaviour formerGripper)
        {
            canUse = false;
            targetGrip = formerGripper; // Save the grip that dropped the boomerang

            remainingHits = 4;
            rigidbody.velocity = Vector3.zero;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (canBounce == false || canRicochet == false)
                return;

            if (remainingHits > 0)
            {
                remainingHits -= 1;
            }

            directionToGrip = (targetGrip.transform.position - transform.position).normalized;

            if (remainingHits > 0 && targetGrip != null && !targetGrip.isHolding)
            {
                RedirectToGrip();
            }
        }

        private void RedirectToGrip()
        {

            directionToGrip = (targetGrip.transform.position - transform.position).normalized;
            rigidbody.velocity = directionToGrip * rigidbody.velocity.magnitude * 2; // Redirect without losing speed
            StartCoroutine(WaitForASecRicochet());
        }

        private void Update()
        {
            if (targetGrip != null)
            {
                directionToGrip = (targetGrip.transform.position - transform.position).normalized;
            }
            if (remainingHits == 0)
            {
                targetGrip = null;
            }
            if (targetGrip != null && remainingHits > 0 && canBounce && canUse)
            {
                if (Vector2.Distance(transform.position, targetGrip.transform.position) <= activationDistance)
                {
                    transform.position = targetGrip.transform.position;
                    targetGrip.Use(new ActivationPropagation()); // Activate Use() on the GripBehaviour
                    remainingHits = 4;
                    canBounce = false;
                    StartCoroutine(WaitForASec());
                }
            }
        }
    }

    public class FighterLimb : MonoBehaviour
    {
        public bool CanFight = false;
        public float strength = 1;
        public bool Shake = false;

        public void OnCollisionEnter2D(Collision2D col)
        {
            if (CanFight)
            {
                if (col.relativeVelocity.magnitude > 2)
                {
                    if (Shake)
                    {
                        CameraShakeBehaviour.main.Shake(col.relativeVelocity.magnitude * 0.1f, col.contacts[0].point, 1);
                        ModAPI.CreateParticleEffect("Vapor", col.contacts[0].point);
                    }

                    if (col.collider.gameObject.TryGetComponent<LimbBehaviour>(out var limb))
                    {
                        limb.Damage(col.relativeVelocity.magnitude * strength);
                        if (col.relativeVelocity.magnitude > 10 / strength)
                        {
                            ModAPI.CreateParticleEffect("BloodExplosion", col.contacts[0].point).GetComponent<ParticleSystem>().startColor = limb.CirculationBehaviour.GetComputedColor();
                        }
                    }
                }
            }
        }
    }

    public class Fighter : Power
    {
        public static Power SetPower(PersonBehaviour Person, Sprite icon, float strength = 0.5f, bool shake = false)
        {
            var power = Person.gameObject.AddComponent<Fighter>();
            power.Name = "Ability to fight";
            power.Description = "Allows the user to fight with their limbs. \n<color=\"yellow\">When the arms or feet of the user hit an object, it will be knocked back slightly and inflict damage upon it if living";
            power.icon = icon;
            power.targetLimb = TargettedLimb.Internal;
            foreach (var limb in Person.Limbs)
            {
                if (limb.name.Contains("LowerArm") || limb.name.Contains("Foot"))
                {
                    limb.gameObject.AddComponent<FighterLimb>().strength = strength;
                    limb.gameObject.GetComponent<FighterLimb>().Shake = shake;
                }
            }

            return power;
        }

        public override void EnablePower()
        {
            base.EnablePower();
            foreach (var f in GetComponentsInChildren<FighterLimb>())
            {
                f.CanFight = true;
            }
        }

        public void OnEnable()
        {
            foreach (var f in GetComponentsInChildren<FighterLimb>())
            {
                f.CanFight = true;
            }
        }

        public void OnDisable()
        {
            foreach (var f in GetComponentsInChildren<FighterLimb>())
            {
                f.CanFight = false;
            }
        }

        public override void DisablePower()
        {
            base.DisablePower();
            foreach (var f in GetComponentsInChildren<FighterLimb>())
            {
                f.CanFight = false;
            }
        }
    }

    public class SlowHealing : Power
    {
        public bool immortal = false;
        public bool CanRegen = true;
        public bool onCooldown = false;
        public PersonBehaviour person;

        public void OnEnable()
        {
            onCooldown = false;
        }

        public override void EnablePower()
        {
            onCooldown = false;
            base.EnablePower();
        }

        public static Power SetPower(PersonBehaviour Personb, Sprite icon, bool immortal = false)
        {
            var power = Personb.gameObject.AddComponent<SlowHealing>();
            power.person = Personb;
            power.icon = icon;
            power.Name = "Slow Healing";
            power.Description = "Allows the user to heal their wounds and restore their health at a slow rate\n<color=\"yellow\">Heals most damage slowly, heals heart attacks, brain damage, but not bone breakage, it is similar to the speed healing power but much slower and weaker.</color>";
            power.targetLimb = TargettedLimb.Internal;
            power.immortal = immortal;
            return power;
        }

        public void FixedUpdate()
        {
            if (!person.IsAlive() && !immortal || Enabled == false)
            {
                return;
            }
            person.AdrenalineLevel = 100;
            person.PainLevel = 0;
            person.Braindead = false;
            person.BrainDamagedTime = 0;
            person.BrainDamaged = false;
            person.Heartbeat = 100f;
            if (!onCooldown)
            {
                foreach (var limb in person.Limbs)
                {
                    if (limb.CirculationBehaviour.GetAmountOfBlood() < 1)
                        limb.CirculationBehaviour.AddLiquid(Liquid.GetLiquid("BLOOD"), .001f);
                    limb.Numbness = 0;
                    limb.LungsPunctured = false;
                    limb.CirculationBehaviour.IsPump = limb.CirculationBehaviour.WasInitiallyPumping;
                    limb.CirculationBehaviour.BloodFlow = 100f;
                    limb.Health = Mathf.Lerp(limb.Health, limb.InitialHealth, 0.0001f);
                    float factor = Mathf.Pow(0.8f, Time.deltaTime);
                    limb.Numbness = 0f;
                    person.Consciousness = 1;
                    person.OxygenLevel = 1;
                    limb.PhysicalBehaviour.BurnProgress -= Time.deltaTime * 0.6f;
                    limb.SkinMaterialHandler.AcidProgress -= Time.deltaTime * 0.8f;
                    limb.SkinMaterialHandler.RottenProgress -= Time.deltaTime * 0.8f;
                    for (int i = 0; i < limb.SkinMaterialHandler.damagePoints.Length; i++)
                    {
                        limb.SkinMaterialHandler.damagePoints[i].z *= factor;
                    }

                    limb.SkinMaterialHandler.Sync();
                }
                StartCoroutine(CoolingDown(0.03f));
            }
        }

        public IEnumerator CoolingDown(float time)
        {
            onCooldown = true;
            yield return new WaitForSeconds(time);
            onCooldown = false;
        }

    }

    public class SpeedHealing : Power
    {
        public bool immortal = false;
        public bool CanRegen = true;
        public bool onCooldown = false;
        public PersonBehaviour person;

        public void OnEnable()
        {
            onCooldown = false;
        }

        public override void EnablePower()
        {
            onCooldown = false;
            base.EnablePower();
        }

        public static Power SetPower(PersonBehaviour Personb, Sprite icon, bool immortal = false)
        {
            var power = Personb.gameObject.AddComponent<SpeedHealing>();
            power.person = Personb;
            power.icon = icon;
            power.Name = "Speed Healing";
            power.Description = "Allows the user to heal their wounds and restore their health at an accelerated rate\n<color=\"yellow\">Heals all damage, including heart attacks, brain damage, and bone breakage, it is similar to the mending syringe but less potent</color>";
            power.targetLimb = TargettedLimb.Internal;
            power.immortal = immortal;
            return power;
        }

        public override void Start()
        {
            person = GetComponent<PersonBehaviour>();
            foreach (var limb in person.Limbs)
            {
                limb.BreakingThreshold = Mathf.Infinity;
                if (limb.HasJoint)
                {
                    limb.Joint.breakForce = Mathf.Infinity;
                }
            }
            base.Start();
        }

        public void FixedUpdate()
        {
            if (!person.IsAlive() && !immortal || Enabled == false)
            {
                return;
            }
            person.Consciousness = 100;
            person.PainLevel = 0;
            person.AdrenalineLevel = 100;
            person.Braindead = false;
            person.BrainDamagedTime = 0;
            person.BrainDamaged = false;
            person.Heartbeat = 666;
            foreach (var limb in person.Limbs)
            {
                if (limb.CirculationBehaviour.GetAmountOfBlood() < 1)
                    limb.CirculationBehaviour.AddLiquid(Liquid.GetLiquid("BLOOD"), .01f);
                limb.Numbness = 0;
                limb.CirculationBehaviour.BleedingRate *= 0.9f;
                limb.CirculationBehaviour.InternalBleedingIntensity = 0;
                limb.Health = Mathf.Lerp(limb.Health, limb.InitialHealth, 0.0005f);
            }
            if (!onCooldown)
            {
                foreach (var limb in person.Limbs)
                {
                    limb.LungsPunctured = false;
                    limb.Person.Heartbeat = 100;
                    limb.CirculationBehaviour.IsPump = limb.CirculationBehaviour.WasInitiallyPumping;
                    limb.CirculationBehaviour.BloodFlow = 1f;
                    limb.CirculationBehaviour.BleedingRate *= 0.9f;
                    limb.Health = Mathf.Lerp(limb.Health, limb.InitialHealth, 0.0005f);
                    float factor = Mathf.Pow(0.5f, Time.deltaTime);
                    limb.HealBone();
                    limb.Numbness = 0f;
                    person.Consciousness = 1;
                    person.OxygenLevel = 1;
                    limb.PhysicalBehaviour.BurnProgress -= Time.deltaTime * 0.16f;
                    limb.SkinMaterialHandler.AcidProgress -= Time.deltaTime * 0.8f;
                    limb.SkinMaterialHandler.RottenProgress -= Time.deltaTime * 0.8f;
                    for (int i = 0; i < limb.SkinMaterialHandler.damagePoints.Length; i++)
                    {
                        limb.SkinMaterialHandler.damagePoints[i].z *= factor;
                    }

                    limb.SkinMaterialHandler.Sync();
                }
                StartCoroutine(CoolingDown(0.03f));
            }
        }

        public IEnumerator CoolingDown(float time)
        {
            onCooldown = true;
            yield return new WaitForSeconds(time);
            onCooldown = false;
        }

    }

    public class Limbs
    {
        public LimbBehaviour limb;
        public float originalMass;
        public float originalStrength;

        public Limbs(LimbBehaviour limbb, float mass, float stength)
        {
            limb = limbb;
            originalMass = mass;
            originalStrength = stength;
        }
    }

    public class SuperMass : Power
    {
        public List<Limbs> limbs = new List<Limbs>();
        bool firstStart = true;
        public float NewMass = 0.5f;
        bool changeMass = true;
        public IEnumerator ChangeMassAfterSeconds()
        {
            yield return new WaitForSeconds(0.1f);

            if (firstStart)
            {
                firstStart = false;
                foreach (var limb in GetComponent<PersonBehaviour>().Limbs)
                {
                    if (limb.GetComponent<Rigidbody2D>())
                    {
                        limbs.Add(new Limbs(limb, limb.GetComponent<Rigidbody2D>().mass, limb.BaseStrength));
                    }
                }
            }

            foreach (var limb in GetComponent<PersonBehaviour>().Limbs)
            {
                if (changeMass)
                    limb.BaseStrength *= NewMass * 0.25f;

                if (limb.GetComponent<Rigidbody2D>())
                    limb.GetComponent<Rigidbody2D>().mass = NewMass;
            }
        }

        public static Power SetPower(PersonBehaviour Person, Sprite icon, float newMass = 0.5f, bool changeMass = false)
        {
            var power = Person.gameObject.AddComponent<SuperMass>();
            power.Name = "Strength";
            power.Description = "Allows the user to become stronger, increasing the mass of their limbs and making them more powerful. \n<color=\"yellow\">This only increases mass of the human to make moving or damaging it with other objects harder, when disabled the user will return to their original mass.</color>";
            power.icon = icon;
            power.targetLimb = TargettedLimb.Internal;
            power.NewMass = newMass;
            power.changeMass = changeMass;
            return power;
        }

        public override void DisablePower()
        {
            base.DisablePower();

            foreach (var limb in limbs)
            {
                foreach (var limbb in GetComponent<PersonBehaviour>().Limbs)
                {
                    if (limb.limb = limbb)
                    {
                        if (changeMass)
                            limb.limb.BaseStrength = limb.originalStrength;

                        if (limbb.GetComponent<Rigidbody2D>())
                            limbb.GetComponent<Rigidbody2D>().mass = limb.originalMass;
                    }
                }
            }
        }

        public override void EnablePower()
        {
            base.EnablePower();

            if (Enabled == false)
            {
                return;
            }

            if (firstStart)
            {
                firstStart = false;
                foreach (var limb in GetComponent<PersonBehaviour>().Limbs)
                {
                    if (limb.GetComponent<Rigidbody2D>())
                    {
                        limbs.Add(new Limbs(limb, limb.GetComponent<Rigidbody2D>().mass, limb.BaseStrength));
                    }
                }
            }

            StartCoroutine(ChangeMassAfterSeconds());
        }

    }

    public class Cape : MonoBehaviour
    {
        public int numberOfPoints = 20;
        public float pointSpacing = 0.085f;
        public Vector3 offset = new Vector3(-0.1675f, 0.069f, 0);
        public float sizeMultiplier = 1.0f;
        public LineRenderer lineRenderer;
        public GameObject[] capePoints;
        private Vector3[] positions;
        public Texture2D capeTexture;
        public Sprite CapeCollar;
        public GameObject CapeCollarOBJ;

        public static void CreateCapeForPerson(PersonBehaviour person, Texture2D Cape, Sprite capeCollar)
        {
            var cape = person.Limbs[1].gameObject.AddComponent<Cape>();
            cape.capeTexture = Cape;
            cape.CapeCollar = capeCollar;
            cape.CapeCollarOBJ = new GameObject("CapeCollar");
            cape.CapeCollarOBJ.transform.parent = person.Limbs[1].transform;
            cape.CapeCollarOBJ.transform.localPosition = Vector3.zero;
            cape.CapeCollarOBJ.transform.localRotation = Quaternion.identity;
            cape.CapeCollarOBJ.transform.localScale = Vector3.one;
            cape.CapeCollarOBJ.AddComponent<SpriteRenderer>().sprite = capeCollar;
            cape.CapeCollarOBJ.GetComponent<SpriteRenderer>().sortingLayerID = person.Limbs[1].GetComponent<SpriteRenderer>().sortingLayerID;
            cape.CapeCollarOBJ.GetComponent<SpriteRenderer>().sortingOrder = person.Limbs[1].GetComponent<SpriteRenderer>().sortingOrder + 1;
            cape.lineRenderer = cape.gameObject.AddComponent<LineRenderer>();
            cape.lineRenderer.positionCount = cape.numberOfPoints;
            cape.lineRenderer.startWidth = 0.175f * cape.sizeMultiplier;
            cape.lineRenderer.endWidth = 0.175f * cape.sizeMultiplier;

            if (cape.capeTexture != null)
            {
                Material material = new Material(Shader.Find("Sprites/Default"));
                material.mainTexture = cape.capeTexture;
                cape.lineRenderer.material = material;
                cape.lineRenderer.startColor = Color.white;
                cape.lineRenderer.endColor = Color.white;
            }
            else
            {
                cape.lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                cape.lineRenderer.startColor = Color.red;
                cape.lineRenderer.endColor = Color.red;
            }

            cape.lineRenderer.useWorldSpace = false;
            cape.lineRenderer.alignment = LineAlignment.View;

            cape.capePoints = new GameObject[cape.numberOfPoints];
            cape.positions = new Vector3[cape.numberOfPoints];

            for (int i = 0; i < cape.numberOfPoints; i++)
            {
                GameObject point = new GameObject("CapePoint" + i);
                point.transform.parent = cape.transform;
                if (i == 0)
                {
                    point.transform.localPosition = cape.offset;
                }
                else
                {
                    point.transform.localPosition = cape.offset + new Vector3(0, -i * cape.pointSpacing * cape.sizeMultiplier, 0);
                }

                Rigidbody2D rb = point.AddComponent<Rigidbody2D>();
                rb.gravityScale = 1f;
                rb.mass = 0.001f;
                rb.drag = 1.2f;
                CircleCollider2D collider = point.AddComponent<CircleCollider2D>();
                collider.radius = 0.05f * cape.sizeMultiplier;

                cape.capePoints[i] = point;
                cape.positions[i] = point.transform.localPosition;
            }

            FixedJoint2D firstJoint = cape.capePoints[0].AddComponent<FixedJoint2D>();
            firstJoint.connectedBody = cape.GetComponent<Rigidbody2D>();

            for (int i = 0; i < cape.numberOfPoints - 1; i++)
            {
                DistanceJoint2D joint = cape.capePoints[i].AddComponent<DistanceJoint2D>();
                joint.connectedBody = cape.capePoints[i + 1].GetComponent<Rigidbody2D>();
                cape.capePoints[i + 1].GetComponent<Rigidbody2D>().interpolation = RigidbodyInterpolation2D.Interpolate;
                joint.autoConfigureDistance = false;
                joint.distance = cape.pointSpacing * cape.sizeMultiplier;
            }
        }

        public void Update()
        {
            CapeCollarOBJ.GetComponent<SpriteRenderer>().sprite = CapeCollar;
            lineRenderer.material.mainTexture = capeTexture;

            Vector3[] positions = new Vector3[(numberOfPoints - 1) * 10 + 1];

            for (int i = 0; i < numberOfPoints - 1; i++)
            {
                Vector3 p0 = (i > 0) ? capePoints[i - 1].transform.localPosition : capePoints[i].transform.localPosition;
                Vector3 p1 = capePoints[i].transform.localPosition;
                Vector3 p2 = capePoints[i + 1].transform.localPosition;
                Vector3 p3 = (i < numberOfPoints - 2) ? capePoints[i + 2].transform.localPosition : capePoints[i + 1].transform.localPosition;

                for (int j = 0; j < 10; j++)
                {
                    float t = j / 10f;
                    positions[i * 10 + j] = 0.5f * ((2 * p1) + (-p0 + p2) * t + (2 * p0 - 5 * p1 + 4 * p2 - p3) * t * t + (-p0 + 3 * p1 - 3 * p2 + p3) * t * t * t);
                }
            }

            positions[positions.Length - 1] = capePoints[numberOfPoints - 1].transform.localPosition;

            lineRenderer.positionCount = positions.Length;
            lineRenderer.SetPositions(positions);

            if (transform.root.localScale.x < 0)
            {
                lineRenderer.widthMultiplier = -1f;
            }
            else
            {
                lineRenderer.widthMultiplier = 1f;
            }
        }
    }

    //----------------------
    // Hello! if you are thinking about using any of my code, feel free to contact me on discord (_timtams.) using any of my code without my permission will get your mod taken down :P
    //----------------------

    public class SkinsDictionary
    {
        public List<Sprite> icons = new List<Sprite>();
        public string characterName;
    }

    public class SubcategorySpawnableEnabler : MonoBehaviour
    {
        public bool Open = false;
        public List<GameObject> spawnables = new List<GameObject>();

        public void OnEnable()
        {
            foreach (var spawn in spawnables)
            {
                spawn.SetActive(Open);
            }
        }

        public void Update()
        {
            foreach (var spawn in spawnables)
            {
                spawn.SetActive(Open);
                spawn.SetActive(Open);
                spawn.SetActive(Open);
                spawn.SetActive(Open);
                spawn.SetActive(Open);
                spawn.SetActive(Open);
                spawn.SetActive(Open);
                spawn.SetActive(Open);
            }
        }
    }

    public class CategoryButtonEditor : MonoBehaviour
    {
        public ItemButtonBehaviour GetIcon(string spawnableAsset)
        {
            foreach (ItemButtonBehaviour item in GameObject.FindObjectsOfType<ItemButtonBehaviour>(true)) { if (item.Item != null) if (item.Item.name == spawnableAsset) return item; }
            return null;
        }

        public List<ItemButtonBehaviour> GetIconsByOrderOverride(string subcategory)
        {
            List<ItemButtonBehaviour> items = new List<ItemButtonBehaviour>();
            foreach (ItemButtonBehaviour item in GameObject.FindObjectsOfType<ItemButtonBehaviour>(true)) { if (item.Item != null) if (item.Item.NameToOrderBy.Contains(subcategory)) items.Add(item); }
            return items;
        }

        public void Start()
        {
            foreach (var name in Mod.skinsIcons)
            {
                if (GetIcon(name.characterName) != null)
                {
                    if (!GetIcon(name.characterName).GetComponent<SkinIconChanger>())
                        GetIcon(name.characterName).gameObject.AddComponent<SkinIconChanger>().icons = name.icons;
                }
            }
        }
    }

    public class SkinIconChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public List<Sprite> icons = new List<Sprite>();
        public Image targetImage;

        private bool hovering = false;
        private float timer = 0f;
        private int currentIndex = 0;

        public void Start()
        {
            targetImage = GetComponent<Image>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            hovering = true;
            timer = 0f;
        }

        public void OnPointerClick(PointerEventData eventData)
        {

        }

        public void OnPointerExit(PointerEventData eventData)
        {
            hovering = false;
            timer = 0f;
        }

        public void Update()
        {
            if (!hovering || icons.Count == 0 || targetImage == null)
                return;

            timer += Time.unscaledDeltaTime;

            if (timer >= 1f)
            {
                timer = 0f;

                currentIndex = (currentIndex + 1) % icons.Count;
                targetImage.sprite = icons[currentIndex];
            }
        }
    }

    public class SpriteMergerAnimator : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        private Texture2D baseTexture;
        private Texture2D overlayTexture;
        private List<Vector2Int> remainingPixels;
        private Material material;

        private Texture2D centeredFleshTex;
        private Texture2D centeredBoneTex;
        private Texture2D centeredDamageTex;

        private Rect finalRect;
        private bool removeExcessTransparent;

        public void Initialize(Sprite baseSprite, Sprite overlaySprite, Material material, bool removeExcessTransparent = true, float Speedd = 0.00001f)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            this.material = material;
            this.removeExcessTransparent = removeExcessTransparent;

            int finalWidth = Mathf.Max((int)baseSprite.rect.width, (int)overlaySprite.rect.width);
            int finalHeight = Mathf.Max((int)baseSprite.rect.height, (int)overlaySprite.rect.height);
            finalRect = new Rect(0, 0, finalWidth, finalHeight);

            baseTexture = CreateTextureFromSprite(baseSprite, finalRect);
            overlayTexture = CreateTextureFromSprite(overlaySprite, finalRect);

            centeredFleshTex = CreateCenteredTexture(material.GetTexture("_FleshTex") as Texture2D, baseSprite.rect, finalRect);
            centeredBoneTex = CreateCenteredTexture(material.GetTexture("_BoneTex") as Texture2D, baseSprite.rect, finalRect);
            centeredDamageTex = CreateCenteredDamageTexture(Mod.DamageTexture, finalRect);

            remainingPixels = new List<Vector2Int>();
            for (int y = 0; y < overlayTexture.height; y++)
            {
                for (int x = 0; x < overlayTexture.width; x++)
                {
                    Color overlayPixel = overlayTexture.GetPixel(x, y);
                    Color basePixel = baseTexture.GetPixel(x, y);

                    if (overlayPixel.a > 0 || (removeExcessTransparent && basePixel.a > 0))
                    {
                        remainingPixels.Add(new Vector2Int(x, y));
                    }
                }
            }

            StartCoroutine(MergeSpritesOverTime(Speedd));
        }

        private IEnumerator MergeSpritesOverTime(float Speed)
        {
            while (remainingPixels.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, remainingPixels.Count);
                Vector2Int pixelPos = remainingPixels[randomIndex];
                remainingPixels.RemoveAt(randomIndex);

                Color overlayColor = overlayTexture.GetPixel(pixelPos.x, pixelPos.y);
                Color baseColor = baseTexture.GetPixel(pixelPos.x, pixelPos.y);

                if (overlayColor.a > 0)
                {
                    baseTexture.SetPixel(pixelPos.x, pixelPos.y, overlayColor);
                }
                else if (removeExcessTransparent && baseColor.a > 0)
                {
                    baseTexture.SetPixel(pixelPos.x, pixelPos.y, new Color(0, 0, 0, 0));
                }

                UpdateShaderTextures(pixelPos);
                baseTexture.Apply();
                spriteRenderer.sprite = Sprite.Create(baseTexture, finalRect, new Vector2(0.5f, 0.5f), spriteRenderer.sprite.pixelsPerUnit);

                yield return new WaitForSeconds(0.01f);
            }
            gameObject.GetComponent<PhysicalBehaviour>().RefreshOutline();

            Destroy(this);
        }

        private void UpdateShaderTextures(Vector2Int pixelPos)
        {
            int xOffset = (baseTexture.width - centeredFleshTex.width) / 2;
            int yOffset = (baseTexture.height - centeredFleshTex.height) / 2;

            if (pixelPos.x - xOffset >= 0 && pixelPos.x - xOffset < centeredFleshTex.width &&
                pixelPos.y - yOffset >= 0 && pixelPos.y - yOffset < centeredFleshTex.height)
            {
                Color fleshColor = centeredFleshTex.GetPixel(pixelPos.x - xOffset, pixelPos.y - yOffset);
                centeredFleshTex.SetPixel(pixelPos.x - xOffset, pixelPos.y - yOffset, fleshColor);

                Color boneColor = centeredBoneTex.GetPixel(pixelPos.x - xOffset, pixelPos.y - yOffset);
                centeredBoneTex.SetPixel(pixelPos.x - xOffset, pixelPos.y - yOffset, boneColor);

                Color damageColor = centeredDamageTex.GetPixel(pixelPos.x - xOffset, pixelPos.y - yOffset);
                centeredDamageTex.SetPixel(pixelPos.x - xOffset, pixelPos.y - yOffset, damageColor);
            }

            centeredFleshTex.Apply();
            centeredBoneTex.Apply();
            centeredDamageTex.Apply();

            material.SetTexture("_FleshTex", centeredFleshTex);
            material.SetTexture("_BoneTex", centeredBoneTex);
            material.SetTexture("_DamageTex", centeredDamageTex);
        }

        private Texture2D CreateTextureFromSprite(Sprite sprite, Rect targetRect)
        {
            int width = (int)targetRect.width;
            int height = (int)targetRect.height;

            Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            texture.filterMode = FilterMode.Point;
            texture.anisoLevel = 0;

            Color clearColor = new Color(0, 0, 0, 0);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    texture.SetPixel(x, y, clearColor);
                }
            }

            int xOffset = (width - (int)sprite.rect.width) / 2;
            int yOffset = (height - (int)sprite.rect.height) / 2;

            for (int y = 0; y < (int)sprite.rect.height; y++)
            {
                for (int x = 0; x < (int)sprite.rect.width; x++)
                {
                    Color color = sprite.texture.GetPixel((int)sprite.rect.x + x, (int)sprite.rect.y + y);
                    texture.SetPixel(x + xOffset, y + yOffset, color);
                }
            }

            texture.Apply();
            return texture;
        }

        private Texture2D CreateCenteredTexture(Texture2D originalTexture, Rect originalRect, Rect targetRect)
        {
            int width = (int)targetRect.width;
            int height = (int)targetRect.height;

            Texture2D newTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            newTexture.filterMode = FilterMode.Point;
            newTexture.anisoLevel = 0;

            Color clearColor = new Color(0, 0, 0, 0);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    newTexture.SetPixel(x, y, clearColor);
                }
            }

            int xOffset = (width - (int)originalRect.width) / 2;
            int yOffset = (height - (int)originalRect.height) / 2;

            for (int y = 0; y < (int)originalRect.height; y++)
            {
                for (int x = 0; x < (int)originalRect.width; x++)
                {
                    Color color = originalTexture.GetPixel((int)originalRect.x + x, (int)originalRect.y + y);
                    newTexture.SetPixel(x + xOffset, y + yOffset, color);
                }
            }

            newTexture.Apply();
            return newTexture;
        }

        private Texture2D CreateCenteredDamageTexture(Texture2D damageTexture, Rect targetRect)
        {
            int width = (int)targetRect.width;
            int height = (int)targetRect.height;

            Texture2D newTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            newTexture.filterMode = FilterMode.Point;
            newTexture.anisoLevel = 0;

            Color clearColor = new Color(0, 0, 0, 0);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    newTexture.SetPixel(x, y, clearColor);
                }
            }

            int xOffset = (width - damageTexture.width) / 2;
            int yOffset = (height - damageTexture.height) / 2;

            for (int y = 0; y < damageTexture.height; y++)
            {
                for (int x = 0; x < damageTexture.width; x++)
                {
                    if (x + xOffset >= 0 && x + xOffset < width && y + yOffset >= 0 && y + yOffset < height)
                    {
                        Color color = damageTexture.GetPixel(x, y);
                        newTexture.SetPixel(x + xOffset, y + yOffset, color);
                    }
                }
            }

            newTexture.Apply();
            return newTexture;
        }
    }

    public class NoCollidea : MonoBehaviour
    {
        public GameObject other;
        public Collider2D col;

        public void FixedUpdate()
        {
            if (other.GetComponent<Collider2D>() != col)
            {
                col = other.GetComponent<Collider2D>();
                foreach (Collider2D collider in GetComponents<Collider2D>())
                {
                    foreach (var collider2 in other.GetComponents<Collider2D>())
                    {
                        Physics2D.IgnoreCollision(collider, collider2, true);
                    }
                }

            }
        }
    }

    public static class Timtam
    {
        public static void CreateCollider(SpriteRenderer sr, float alphaThreshold = 0.1f)
        {
            if (sr == null || sr.sprite == null)
                return;

            var go = sr.gameObject;
            var collider = go.GetComponent<PolygonCollider2D>() ?? go.AddComponent<PolygonCollider2D>();
            collider.pathCount = 0;

            Sprite sprite = sr.sprite;
            Texture2D tex = sprite.texture;
            if (!tex.isReadable)
                return;

            Rect tRect = sprite.textureRect;
            Vector2 tOffset = sprite.textureRectOffset;
            int w = (int)tRect.width;
            int h = (int)tRect.height;

            Color[] pixels = tex.GetPixels((int)tRect.x, (int)tRect.y, w, h);
            bool[,] mask = new bool[w, h];
            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                    mask[x, y] = pixels[y * w + x].a >= alphaThreshold;

            var segments = new Dictionary<Edge, int>();
            void AddSeg(int x1, int y1, int x2, int y2)
            {
                var e = new Edge(x1, y1, x2, y2);
                if (segments.ContainsKey(e)) segments[e]++;
                else segments[e] = 1;
            }

            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                {
                    if (!mask[x, y]) continue;
                    if (y == 0 || !mask[x, y - 1]) AddSeg(x, y, x + 1, y);
                    if (y == h - 1 || !mask[x, y + 1]) AddSeg(x + 1, y + 1, x, y + 1);
                    if (x == 0 || !mask[x - 1, y]) AddSeg(x, y + 1, x, y);
                    if (x == w - 1 || !mask[x + 1, y]) AddSeg(x + 1, y, x + 1, y + 1);
                }

            var border = new List<Edge>();
            foreach (var kv in segments)
                if (kv.Value == 1)
                    border.Add(kv.Key);

            var loops = BuildLoops(border);

            float ppu = sprite.pixelsPerUnit;
            Vector2 pivot = sprite.pivot;
            collider.pathCount = loops.Count;

            for (int i = 0; i < loops.Count; i++)
            {
                var pts = loops[i];
                var path = new Vector2[pts.Count];
                for (int j = 0; j < pts.Count; j++)
                {
                    Vector2 fullSpacePx = new Vector2(pts[j].x, pts[j].y) + tOffset;
                    path[j] = (fullSpacePx - pivot) / ppu;
                }
                collider.SetPath(i, path);
            }
        }

        private struct Edge
        {
            public readonly int x1, y1, x2, y2;
            public Edge(int ax, int ay, int bx, int by)
            {
                if (ax < bx || (ax == bx && ay < by))
                {
                    x1 = ax; y1 = ay; x2 = bx; y2 = by;
                }
                else
                {
                    x1 = bx; y1 = by; x2 = ax; y2 = ay;
                }
            }
            public override bool Equals(object obj)
            {
                return obj is Edge e && e.x1 == x1 && e.y1 == y1 && e.x2 == x2 && e.y2 == y2;
            }
            public override int GetHashCode()
            {
                unchecked { return (x1 * 397 ^ y1) * 397 ^ x2 * 397 ^ y2; }
            }
        }

        private static List<List<Vector2>> BuildLoops(List<Edge> edges)
        {
            var loops = new List<List<Vector2>>();
            var edgeList = new List<Edge>(edges);

            while (edgeList.Count > 0)
            {
                var loop = new List<Vector2>();
                var e = edgeList[0]; edgeList.RemoveAt(0);
                Vector2 start = new Vector2(e.x1, e.y1);
                Vector2 curr = new Vector2(e.x2, e.y2);
                loop.Add(start);
                loop.Add(curr);

                bool closed = false;
                while (!closed)
                {
                    bool match = false;
                    for (int i = 0; i < edgeList.Count; i++)
                    {
                        var ne = edgeList[i];
                        Vector2 a = new Vector2(ne.x1, ne.y1);
                        Vector2 b = new Vector2(ne.x2, ne.y2);
                        if (a == curr || b == curr)
                        {
                            Vector2 next = (a == curr) ? b : a;
                            loop.Add(next);
                            curr = next;
                            edgeList.RemoveAt(i);
                            match = true;
                            if (curr == start) closed = true;
                            break;
                        }
                    }
                    if (!match) break;
                }

                loops.Add(loop);
            }

            return loops;
        }
        public static Sprite MergeSpritesWithShaderHandling(Sprite baseSprite, Sprite overlaySprite, Material material)
        {
            if (baseSprite == null || overlaySprite == null || material == null)
                return baseSprite;

            Sprite mergedSprite = MergeSprites(baseSprite, overlaySprite);

            Texture2D fleshTex = material.GetTexture("_FleshTex") as Texture2D;
            Texture2D boneTex = material.GetTexture("_BoneTex") as Texture2D;

            Texture2D centeredFleshTex = CreateCenteredTexture(fleshTex, baseSprite.rect, mergedSprite.rect);
            Texture2D centeredBoneTex = CreateCenteredTexture(boneTex, baseSprite.rect, mergedSprite.rect);

            material.SetTexture("_FleshTex", centeredFleshTex);
            material.SetTexture("_BoneTex", centeredBoneTex);

            Texture2D damageTex = Mod.DamageTexture as Texture2D;
            Texture2D centeredDamageTex = CreateCenteredDamageTexture(damageTex, mergedSprite.rect);

            material.SetTexture("_DamageTex", centeredDamageTex);

            return mergedSprite;
        }

        private static Texture2D CreateCenteredTexture(Texture2D originalTexture, Rect originalRect, Rect targetRect)
        {
            int width = (int)targetRect.width;
            int height = (int)targetRect.height;

            Texture2D newTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            newTexture.filterMode = FilterMode.Point;
            newTexture.anisoLevel = 0;

            Color clearColor = new Color(0, 0, 0, 0);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    newTexture.SetPixel(x, y, clearColor);
                }
            }

            int xOffset = (width - (int)originalRect.width) / 2;
            int yOffset = (height - (int)originalRect.height) / 2;

            for (int y = 0; y < (int)originalRect.height; y++)
            {
                for (int x = 0; x < (int)originalRect.width; x++)
                {
                    Color color = originalTexture.GetPixel((int)originalRect.x + x, (int)originalRect.y + y);
                    newTexture.SetPixel(x + xOffset, y + yOffset, color);
                }
            }

            newTexture.Apply();
            return newTexture;
        }

        private static Texture2D CreateCenteredDamageTexture(Texture2D damageTexture, Rect targetRect)
        {
            int width = (int)targetRect.width;
            int height = (int)targetRect.height;

            Texture2D newTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            newTexture.filterMode = FilterMode.Point;
            newTexture.anisoLevel = 0;

            Color clearColor = new Color(0, 0, 0, 0);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    newTexture.SetPixel(x, y, clearColor);
                }
            }

            int xOffset = (width - damageTexture.width) / 2;
            int yOffset = (height - damageTexture.height) / 2;

            for (int y = 0; y < damageTexture.height; y++)
            {
                for (int x = 0; x < damageTexture.width; x++)
                {
                    if (x + xOffset >= 0 && x + xOffset < width && y + yOffset >= 0 && y + yOffset < height)
                    {
                        Color color = damageTexture.GetPixel(x, y);
                        newTexture.SetPixel(x + xOffset, y + yOffset, color);
                    }
                }
            }

            newTexture.Apply();
            return newTexture;
        }

        public static Sprite MergeSprites(Sprite baseSprite, Sprite overlaySprite)
        {
            if (baseSprite == null || overlaySprite == null)
                return baseSprite;

            Texture2D baseTexture = baseSprite.texture;
            Texture2D overlayTexture = overlaySprite.texture;

            Rect baseRect = baseSprite.rect;
            Rect overlayRect = overlaySprite.rect;

            int width = Mathf.Max((int)baseRect.width, (int)overlayRect.width);
            int height = Mathf.Max((int)baseRect.height, (int)overlayRect.height);

            Texture2D mergedTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            mergedTexture.filterMode = FilterMode.Point;
            mergedTexture.anisoLevel = 0;

            Color clearColor = new Color(0, 0, 0, 0);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    mergedTexture.SetPixel(x, y, clearColor);
                }
            }

            int baseXOffset = (width - (int)baseRect.width) / 2;
            int baseYOffset = (height - (int)baseRect.height) / 2;

            for (int y = 0; y < (int)baseRect.height; y++)
            {
                for (int x = 0; x < (int)baseRect.width; x++)
                {
                    Color baseColor = baseTexture.GetPixel((int)baseRect.x + x, (int)baseRect.y + y);
                    mergedTexture.SetPixel(baseXOffset + x, baseYOffset + y, baseColor);
                }
            }

            int overlayXOffset = (width - (int)overlayRect.width) / 2;
            int overlayYOffset = (height - (int)overlayRect.height) / 2;

            for (int y = 0; y < (int)overlayRect.height; y++)
            {
                for (int x = 0; x < (int)overlayRect.width; x++)
                {
                    Color overlayColor = overlayTexture.GetPixel((int)overlayRect.x + x, (int)overlayRect.y + y);
                    if (overlayColor.a > 0)
                    {
                        mergedTexture.SetPixel(overlayXOffset + x, overlayYOffset + y, overlayColor);
                    }
                }
            }

            mergedTexture.Apply();

            Sprite mergedSprite = Sprite.Create(mergedTexture,
                new Rect(0, 0, mergedTexture.width, mergedTexture.height),
                new Vector2(0.5f, 0.5f),
                baseSprite.pixelsPerUnit);

            return mergedSprite;
        }

        public static Sprite MergeSpritesWithShaderHandling(Sprite baseSprite, Sprite overlaySprite, Material material, bool preserveBaseTransparency, bool IgnoreTransparentOnOverlay = false)
        {
            if (baseSprite == null || overlaySprite == null || material == null)
                return baseSprite;

            Sprite mergedSprite;
            if (IgnoreTransparentOnOverlay)
            {
                mergedSprite = MergeSpritesWithTransparency(baseSprite, overlaySprite, preserveBaseTransparency);
            }
            else
            {
                mergedSprite = MergeSprites(baseSprite, overlaySprite, preserveBaseTransparency);
            }

            Texture2D fleshTex = material.GetTexture("_FleshTex") as Texture2D;
            Texture2D boneTex = material.GetTexture("_BoneTex") as Texture2D;

            Texture2D centeredFleshTex = CreateCenteredTexture(fleshTex, baseSprite.rect, mergedSprite.rect);
            Texture2D centeredBoneTex = CreateCenteredTexture(boneTex, baseSprite.rect, mergedSprite.rect);

            material.SetTexture("_FleshTex", centeredFleshTex);
            material.SetTexture("_BoneTex", centeredBoneTex);

            Texture2D damageTex = Mod.DamageTexture as Texture2D;
            Texture2D centeredDamageTex = CreateCenteredDamageTexture(damageTex, mergedSprite.rect);

            material.SetTexture("_DamageTex", centeredDamageTex);

            return mergedSprite;
        }

        public static Sprite MergeSprites(Sprite baseSprite, Sprite overlaySprite, bool preserveBaseTransparency)
        {
            if (baseSprite == null || overlaySprite == null)
                return baseSprite;

            Texture2D baseTexture = baseSprite.texture;
            Texture2D overlayTexture = overlaySprite.texture;

            Rect baseRect = baseSprite.rect;
            Rect overlayRect = overlaySprite.rect;

            int width = Mathf.Max((int)baseRect.width, (int)overlayRect.width);
            int height = Mathf.Max((int)baseRect.height, (int)overlayRect.height);

            Texture2D mergedTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            mergedTexture.filterMode = FilterMode.Point;
            mergedTexture.anisoLevel = 0;

            Color clearColor = new Color(0, 0, 0, 0);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    mergedTexture.SetPixel(x, y, clearColor);
                }
            }

            int baseXOffset = (width - (int)baseRect.width) / 2;
            int baseYOffset = (height - (int)baseRect.height) / 2;

            // Copy base sprite pixels
            for (int y = 0; y < (int)baseRect.height; y++)
            {
                for (int x = 0; x < (int)baseRect.width; x++)
                {
                    Color baseColor = baseTexture.GetPixel((int)baseRect.x + x, (int)baseRect.y + y);
                    mergedTexture.SetPixel(baseXOffset + x, baseYOffset + y, baseColor);
                }
            }

            int overlayXOffset = (width - (int)overlayRect.width) / 2;
            int overlayYOffset = (height - (int)overlayRect.height) / 2;

            // Merge overlay sprite with alpha blending
            for (int y = 0; y < (int)overlayRect.height; y++)
            {
                for (int x = 0; x < (int)overlayRect.width; x++)
                {
                    Color overlayColor = overlayTexture.GetPixel((int)overlayRect.x + x, (int)overlayRect.y + y);
                    Color baseColor = mergedTexture.GetPixel(overlayXOffset + x, overlayYOffset + y);

                    if (overlayColor.a > 0)  // Only blend if the overlay pixel isn't fully transparent
                    {
                        if (baseColor.a > 0 || !preserveBaseTransparency)
                        {
                            // Alpha blending: overlay color blended with base color based on overlay alpha
                            float finalAlpha = overlayColor.a + baseColor.a * (1 - overlayColor.a);
                            Color blendedColor = (overlayColor * overlayColor.a + baseColor * baseColor.a * (1 - overlayColor.a)) / finalAlpha;
                            blendedColor.a = finalAlpha;  // Ensure the final alpha is correctly calculated
                            mergedTexture.SetPixel(overlayXOffset + x, overlayYOffset + y, blendedColor);
                        }
                    }
                }
            }

            mergedTexture.Apply();

            Sprite mergedSprite = Sprite.Create(mergedTexture,
                new Rect(0, 0, mergedTexture.width, mergedTexture.height),
                new Vector2(0.5f, 0.5f),
                baseSprite.pixelsPerUnit);

            return mergedSprite;
        }

        public static Sprite MergeSpritesWithTransparency(Sprite baseSprite, Sprite overlaySprite, bool preserveBaseTransparency = false, bool setBaseToOverlayTransparency = false)
        {
            if (baseSprite == null || overlaySprite == null)
                return baseSprite;

            Texture2D baseTexture = baseSprite.texture;
            Texture2D overlayTexture = overlaySprite.texture;

            Rect baseRect = baseSprite.rect;
            Rect overlayRect = overlaySprite.rect;

            int width = Mathf.Max((int)baseRect.width, (int)overlayRect.width);
            int height = Mathf.Max((int)baseRect.height, (int)overlayRect.height);

            Texture2D mergedTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            mergedTexture.filterMode = FilterMode.Point;
            mergedTexture.anisoLevel = 0;

            Color clearColor = new Color(0, 0, 0, 0);

            // Initialize mergedTexture to fully transparent
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    mergedTexture.SetPixel(x, y, clearColor);
                }
            }

            int baseXOffset = (width - (int)baseRect.width) / 2;
            int baseYOffset = (height - (int)baseRect.height) / 2;

            // Copy base sprite pixels
            for (int y = 0; y < (int)baseRect.height; y++)
            {
                for (int x = 0; x < (int)baseRect.width; x++)
                {
                    Color baseColor = baseTexture.GetPixel((int)baseRect.x + x, (int)baseRect.y + y);
                    mergedTexture.SetPixel(baseXOffset + x, baseYOffset + y, baseColor);
                }
            }

            int overlayXOffset = (width - (int)overlayRect.width) / 2;
            int overlayYOffset = (height - (int)overlayRect.height) / 2;

            // Merge overlay sprite with alpha blending and transparency checks
            for (int y = 0; y < (int)overlayRect.height; y++)
            {
                for (int x = 0; x < (int)overlayRect.width; x++)
                {
                    Color overlayColor = overlayTexture.GetPixel((int)overlayRect.x + x, (int)overlayRect.y + y);
                    Color baseColor = mergedTexture.GetPixel(overlayXOffset + x, overlayYOffset + y);

                    if (overlayColor.a > 0)  // Only blend if the overlay pixel isn't fully transparent
                    {
                        if (baseColor.a > 0 || !preserveBaseTransparency)
                        {
                            // Alpha blending: overlay color blended with base color based on overlay alpha
                            float finalAlpha = overlayColor.a + baseColor.a * (1 - overlayColor.a);
                            Color blendedColor = (overlayColor * overlayColor.a + baseColor * baseColor.a * (1 - overlayColor.a)) / finalAlpha;
                            blendedColor.a = finalAlpha;  // Ensure the final alpha is correctly calculated
                            mergedTexture.SetPixel(overlayXOffset + x, overlayYOffset + y, blendedColor);
                        }
                    }
                    else
                    {
                        // If overlay is fully transparent, make the corresponding base pixel transparent
                        mergedTexture.SetPixel(overlayXOffset + x, overlayYOffset + y, clearColor);
                    }
                }
            }

            // Set all base sprite pixels outside overlay bounds to transparent
            for (int y = 0; y < (int)baseRect.height; y++)
            {
                for (int x = 0; x < (int)baseRect.width; x++)
                {
                    if (x + baseXOffset < overlayXOffset || x + baseXOffset >= overlayXOffset + (int)overlayRect.width ||
                        y + baseYOffset < overlayYOffset || y + baseYOffset >= overlayYOffset + (int)overlayRect.height)
                    {
                        mergedTexture.SetPixel(baseXOffset + x, baseYOffset + y, clearColor);
                    }
                }
            }

            mergedTexture.Apply();

            Sprite mergedSprite = Sprite.Create(mergedTexture,
                new Rect(0, 0, mergedTexture.width, mergedTexture.height),
                new Vector2(0.5f, 0.5f),
                baseSprite.pixelsPerUnit);

            return mergedSprite;
        }

        public static void MakeCustomSkin(PersonBehaviour person, Sprite HeadSprite, Sprite UpperBodySprite, Sprite MiddleBodySprite, Sprite LowerBodySprite, Sprite UpperArmSprite, Sprite LowerArmSprite, Sprite UpperLegSprite, Sprite LowerLegSprite, Sprite FootSprite, bool WrapOverBasically = false)
        {
            foreach (var limb in person.Limbs)
            {
                if (limb.name.Contains("Head"))
                {
                    if (HeadSprite != null)
                    {
                        var Head = limb.GetComponent<SpriteRenderer>();
                        Head.sprite = Timtam.MergeSpritesWithShaderHandling(Head.sprite, HeadSprite, Head.material, WrapOverBasically);
                        Head.GetComponent<PhysicalBehaviour>().RefreshOutline();
                    }
                }
                else if (limb.name.Contains("UpperBody"))
                {
                    if (UpperBodySprite != null)
                    {
                        var Head = limb.GetComponent<SpriteRenderer>();
                        Head.sprite = Timtam.MergeSpritesWithShaderHandling(Head.sprite, UpperBodySprite, Head.material, WrapOverBasically);
                        Head.GetComponent<PhysicalBehaviour>().RefreshOutline();
                    }
                }
                else if (limb.name.Contains("MiddleBody"))
                {
                    if (MiddleBodySprite != null)
                    {
                        var Head = limb.GetComponent<SpriteRenderer>();
                        Head.sprite = Timtam.MergeSpritesWithShaderHandling(Head.sprite, MiddleBodySprite, Head.material, WrapOverBasically);
                        Head.GetComponent<PhysicalBehaviour>().RefreshOutline();
                    }
                }
                else if (limb.name.Contains("LowerBody"))
                {
                    if (LowerBodySprite != null)
                    {
                        var Head = limb.GetComponent<SpriteRenderer>();
                        Head.sprite = Timtam.MergeSpritesWithShaderHandling(Head.sprite, LowerBodySprite, Head.material, WrapOverBasically);
                        Head.GetComponent<PhysicalBehaviour>().RefreshOutline();
                    }
                }
                else if (limb.name.Contains("UpperArm"))
                {
                    if (UpperArmSprite != null)
                    {
                        var Head = limb.GetComponent<SpriteRenderer>();
                        Head.sprite = Timtam.MergeSpritesWithShaderHandling(Head.sprite, UpperArmSprite, Head.material, WrapOverBasically);
                        Head.GetComponent<PhysicalBehaviour>().RefreshOutline();
                    }
                }
                else if (limb.name.Contains("LowerArm"))
                {
                    if (LowerArmSprite != null)
                    {
                        var Head = limb.GetComponent<SpriteRenderer>();
                        Head.sprite = Timtam.MergeSpritesWithShaderHandling(Head.sprite, LowerArmSprite, Head.material, WrapOverBasically);
                        Head.GetComponent<PhysicalBehaviour>().RefreshOutline();
                    }
                }
                else if (limb.name.Contains("UpperLeg"))
                {
                    if (UpperLegSprite != null)
                    {
                        var Head = limb.GetComponent<SpriteRenderer>();
                        Head.sprite = Timtam.MergeSpritesWithShaderHandling(Head.sprite, UpperLegSprite, Head.material, WrapOverBasically);
                        Head.GetComponent<PhysicalBehaviour>().RefreshOutline();
                    }
                }
                else if (limb.name.Contains("LowerLeg"))
                {
                    if (LowerLegSprite != null)
                    {
                        var Head = limb.GetComponent<SpriteRenderer>();
                        Head.sprite = Timtam.MergeSpritesWithShaderHandling(Head.sprite, LowerLegSprite, Head.material, WrapOverBasically);
                        Head.GetComponent<PhysicalBehaviour>().RefreshOutline();
                    }
                }
                if (limb.name.Contains("Foot"))
                {
                    if (FootSprite != null)
                    {
                        var Head = limb.GetComponent<SpriteRenderer>();
                        Head.sprite = Timtam.MergeSpritesWithShaderHandling(Head.sprite, FootSprite, Head.material, WrapOverBasically);
                        Head.GetComponent<PhysicalBehaviour>().RefreshOutline();
                    }
                }
            }

        }

        public static void MakeCustomSkin(PersonBehaviour person, List<Sprite> sprites, bool WrapOverBasically = false)
        {
            foreach (var limb in person.Limbs)
            {
                foreach (var sprite in sprites)
                {
                    if (limb.name.Contains("Head") && sprite.name == "Head")
                    {
                        var Head = limb.GetComponent<SpriteRenderer>();
                        Head.sprite = Timtam.MergeSpritesWithShaderHandling(Head.sprite, sprite, Head.material, WrapOverBasically);
                        Head.GetComponent<PhysicalBehaviour>().RefreshOutline();

                    }
                    else if (limb.name.Contains("UpperBody") && sprite.name == "UpperBody")
                    {
                        var Head = limb.GetComponent<SpriteRenderer>();
                        Head.sprite = Timtam.MergeSpritesWithShaderHandling(Head.sprite, sprite, Head.material, WrapOverBasically);
                        Head.GetComponent<PhysicalBehaviour>().RefreshOutline();

                    }
                    else if (limb.name.Contains("MiddleBody") && sprite.name == "MiddleBody")
                    {
                        var Head = limb.GetComponent<SpriteRenderer>();
                        Head.sprite = Timtam.MergeSpritesWithShaderHandling(Head.sprite, sprite, Head.material, WrapOverBasically);
                        Head.GetComponent<PhysicalBehaviour>().RefreshOutline();

                    }
                    else if (limb.name.Contains("LowerBody") && sprite.name == "LowerBody")
                    {
                        var Head = limb.GetComponent<SpriteRenderer>();
                        Head.sprite = Timtam.MergeSpritesWithShaderHandling(Head.sprite, sprite, Head.material, WrapOverBasically);
                        Head.GetComponent<PhysicalBehaviour>().RefreshOutline();

                    }
                    else if (limb.name.Contains("UpperArm") && sprite.name == "UpperArm")
                    {
                        var Head = limb.GetComponent<SpriteRenderer>();
                        Head.sprite = Timtam.MergeSpritesWithShaderHandling(Head.sprite, sprite, Head.material, WrapOverBasically);
                        Head.GetComponent<PhysicalBehaviour>().RefreshOutline();
                    }
                    else if (limb.name.Contains("LowerArm") && sprite.name == "LowerArm")
                    {
                        var Head = limb.GetComponent<SpriteRenderer>();
                        Head.sprite = Timtam.MergeSpritesWithShaderHandling(Head.sprite, sprite, Head.material, WrapOverBasically);
                        Head.GetComponent<PhysicalBehaviour>().RefreshOutline();
                    }
                    else if (limb.name.Contains("UpperLeg") && sprite.name == "UpperLeg")
                    {
                        var Head = limb.GetComponent<SpriteRenderer>();
                        Head.sprite = Timtam.MergeSpritesWithShaderHandling(Head.sprite, sprite, Head.material, WrapOverBasically);
                        Head.GetComponent<PhysicalBehaviour>().RefreshOutline();
                    }
                    else if (limb.name.Contains("LowerLeg") && sprite.name == "LowerLeg")
                    {
                        var Head = limb.GetComponent<SpriteRenderer>();
                        Head.sprite = Timtam.MergeSpritesWithShaderHandling(Head.sprite, sprite, Head.material, WrapOverBasically);
                        Head.GetComponent<PhysicalBehaviour>().RefreshOutline();
                    }
                    if (limb.name.Contains("Foot") && sprite.name == "Foot")
                    {
                        var Head = limb.GetComponent<SpriteRenderer>();
                        Head.sprite = Timtam.MergeSpritesWithShaderHandling(Head.sprite, sprite, Head.material, WrapOverBasically);
                        Head.GetComponent<PhysicalBehaviour>().RefreshOutline();
                    }
                }
            }

        }

        public static void MakeCustomSkin(PersonBehaviour person, List<Sprite> sprites, bool WrapOverBasically = false, bool OverlayOnly = false)
        {
            foreach (var limb in person.Limbs)
            {
                foreach (var sprite in sprites)
                {
                    if (limb.name.Contains("Head") && sprite.name == "Head")
                    {
                        var Head = limb.GetComponent<SpriteRenderer>();
                        Head.sprite = Timtam.MergeSpritesWithShaderHandling(Head.sprite, sprite, Head.material, WrapOverBasically, OverlayOnly);
                        Head.GetComponent<PhysicalBehaviour>().RefreshOutline();
                    }
                    else if (limb.name.Contains("UpperBody") && sprite.name == "UpperBody")
                    {
                        var Head = limb.GetComponent<SpriteRenderer>();
                        Head.sprite = Timtam.MergeSpritesWithShaderHandling(Head.sprite, sprite, Head.material, WrapOverBasically, OverlayOnly);
                        Head.GetComponent<PhysicalBehaviour>().RefreshOutline();
                    }
                    else if (limb.name.Contains("MiddleBody") && sprite.name == "MiddleBody")
                    {
                        var Head = limb.GetComponent<SpriteRenderer>();
                        Head.sprite = Timtam.MergeSpritesWithShaderHandling(Head.sprite, sprite, Head.material, WrapOverBasically, OverlayOnly);
                        Head.GetComponent<PhysicalBehaviour>().RefreshOutline();
                    }
                    else if (limb.name.Contains("LowerBody") && sprite.name == "LowerBody")
                    {
                        var Head = limb.GetComponent<SpriteRenderer>();
                        Head.sprite = Timtam.MergeSpritesWithShaderHandling(Head.sprite, sprite, Head.material, WrapOverBasically, OverlayOnly);
                        Head.GetComponent<PhysicalBehaviour>().RefreshOutline();
                    }
                    else if (limb.name == "UpperArm" && sprite.name == "UpperArm")
                    {
                        var Head = limb.GetComponent<SpriteRenderer>();
                        Head.sprite = Timtam.MergeSpritesWithShaderHandling(Head.sprite, sprite, Head.material, WrapOverBasically, OverlayOnly);
                        Head.GetComponent<PhysicalBehaviour>().RefreshOutline();
                    }
                    else if (limb.name == "LowerArm" && sprite.name == "LowerArm")
                    {
                        var Head = limb.GetComponent<SpriteRenderer>();
                        Head.sprite = Timtam.MergeSpritesWithShaderHandling(Head.sprite, sprite, Head.material, WrapOverBasically, OverlayOnly);
                        Head.GetComponent<PhysicalBehaviour>().RefreshOutline();
                    }
                    else if (limb.name == "UpperLeg" && sprite.name == "UpperLeg")
                    {
                        var Head = limb.GetComponent<SpriteRenderer>();
                        Head.sprite = Timtam.MergeSpritesWithShaderHandling(Head.sprite, sprite, Head.material, WrapOverBasically, OverlayOnly);
                        Head.GetComponent<PhysicalBehaviour>().RefreshOutline();
                    }
                    else if (limb.name == "LowerLeg" && sprite.name == "LowerLeg")
                    {
                        var Head = limb.GetComponent<SpriteRenderer>();
                        Head.sprite = Timtam.MergeSpritesWithShaderHandling(Head.sprite, sprite, Head.material, WrapOverBasically, OverlayOnly);
                        Head.GetComponent<PhysicalBehaviour>().RefreshOutline();
                    }
                    else if (limb.name == "Foot" && sprite.name == "Foot")
                    {
                        var Head = limb.GetComponent<SpriteRenderer>();
                        Head.sprite = Timtam.MergeSpritesWithShaderHandling(Head.sprite, sprite, Head.material, WrapOverBasically, OverlayOnly);
                        Head.GetComponent<PhysicalBehaviour>().RefreshOutline();
                    }
                    else if (limb.name.Contains("Front"))
                    {
                        var Head = limb.GetComponent<SpriteRenderer>();
                        var matchingSprite = sprites.FirstOrDefault(s => s.name == limb.name);
                        if (matchingSprite != null)
                        {
                            Head.sprite = Timtam.MergeSpritesWithShaderHandling(Head.sprite, matchingSprite, Head.material, WrapOverBasically, OverlayOnly);
                        }
                        else
                        {
                            var nonFrontSprite = sprites.FirstOrDefault(s => s.name == limb.name.Replace("Front", ""));
                            if (nonFrontSprite != null)
                            {
                                Head.sprite = Timtam.MergeSpritesWithShaderHandling(Head.sprite, nonFrontSprite, Head.material, WrapOverBasically, OverlayOnly);
                            }
                        }
                        Head.GetComponent<PhysicalBehaviour>().RefreshOutline();
                    }
                }
            }
            person.SetBruiseColor(130, 10, 10);             // Purple
            person.SetSecondBruiseColor(180, 20, 20);        // Indigo (deep purple)
            person.SetThirdBruiseColor(139, 10, 10);         // Dark magenta (reddish purple)
            person.SetRottenColour(139, 0, 10);         // Medium orchid (purple tint)
            person.SetBloodColour(139, 0, 10);            // Dark red
        }

        public static void MakeCustomSkinAnimated(PersonBehaviour person, List<Sprite> sprites, bool WrapOverBasically = false, bool OverlayOnly = false)
        {
            foreach (var limb in person.Limbs)
            {
                foreach (var sprite in sprites)
                {
                    if (limb.name.Contains("Head") && sprite.name == "Head")
                    {
                        var Head = limb.GetComponent<SpriteRenderer>();
                        limb.gameObject.AddComponent<SpriteMergerAnimator>().Initialize(Head.sprite, sprite, Head.material, true);
                    }
                    else if (limb.name.Contains("UpperBody") && sprite.name == "UpperBody")
                    {
                        var Head = limb.GetComponent<SpriteRenderer>();
                        limb.gameObject.AddComponent<SpriteMergerAnimator>().Initialize(Head.sprite, sprite, Head.material, true);
                    }
                    else if (limb.name.Contains("MiddleBody") && sprite.name == "MiddleBody")
                    {
                        var Head = limb.GetComponent<SpriteRenderer>();
                        limb.gameObject.AddComponent<SpriteMergerAnimator>().Initialize(Head.sprite, sprite, Head.material, true);
                    }
                    else if (limb.name.Contains("LowerBody") && sprite.name == "LowerBody")
                    {
                        var Head = limb.GetComponent<SpriteRenderer>();
                        limb.gameObject.AddComponent<SpriteMergerAnimator>().Initialize(Head.sprite, sprite, Head.material, true);
                    }
                    else if (limb.name == "UpperArm" && sprite.name == "UpperArm")
                    {
                        var Head = limb.GetComponent<SpriteRenderer>();
                        limb.gameObject.AddComponent<SpriteMergerAnimator>().Initialize(Head.sprite, sprite, Head.material, true);
                    }
                    else if (limb.name == "LowerArm" && sprite.name == "LowerArm")
                    {
                        var Head = limb.GetComponent<SpriteRenderer>();
                        limb.gameObject.AddComponent<SpriteMergerAnimator>().Initialize(Head.sprite, sprite, Head.material, true);
                    }
                    else if (limb.name == "UpperLeg" && sprite.name == "UpperLeg")
                    {
                        var Head = limb.GetComponent<SpriteRenderer>();
                        limb.gameObject.AddComponent<SpriteMergerAnimator>().Initialize(Head.sprite, sprite, Head.material, true);
                    }
                    else if (limb.name == "LowerLeg" && sprite.name == "LowerLeg")
                    {
                        var Head = limb.GetComponent<SpriteRenderer>();
                        limb.gameObject.AddComponent<SpriteMergerAnimator>().Initialize(Head.sprite, sprite, Head.material, true);
                    }
                    else if (limb.name == "Foot" && sprite.name == "Foot")
                    {
                        var Head = limb.GetComponent<SpriteRenderer>();
                        limb.gameObject.AddComponent<SpriteMergerAnimator>().Initialize(Head.sprite, sprite, Head.material, true);
                    }

                    if (limb.name.Contains("Front"))
                    {
                        var Head = limb.GetComponent<SpriteRenderer>();
                        var matchingSprite = sprites.FirstOrDefault(s => s.name == limb.name);
                        if (matchingSprite != null)
                        {
                            Head.gameObject.AddComponent<SpriteMergerAnimator>().Initialize(Head.sprite, sprite, Head.material, true);
                        }
                        else
                        {
                            var nonFrontSprite = sprites.FirstOrDefault(s => s.name == limb.name.Replace("Front", ""));
                            if (nonFrontSprite != null)
                            {
                                Head.gameObject.AddComponent<SpriteMergerAnimator>().Initialize(Head.sprite, nonFrontSprite, Head.material, true);
                            }
                        }
                    }
                }
            }
            person.SetBruiseColor(130, 10, 10);             // Purple
            person.SetSecondBruiseColor(180, 20, 20);        // Indigo (deep purple)
            person.SetThirdBruiseColor(139, 10, 10);         // Dark magenta (reddish purple)
            person.SetRottenColour(139, 0, 10);         // Medium orchid (purple tint)
            person.SetBloodColour(139, 0, 10);            // Dark red
        }

        public static void MakeCustomHead(GameObject Head, Sprite HeadSprite, Texture Flesh, Texture Bone, Texture Damage)
        {
            Head.GetComponent<SpriteRenderer>().sprite = HeadSprite;

            Head.GetComponent<SpriteRenderer>().material.SetTexture("_FleshTex", Flesh);
            Head.GetComponent<SpriteRenderer>().material.SetTexture("_BoneTex", Bone);
            Head.GetComponent<SpriteRenderer>().material.SetTexture("_DamageTex", Damage);

            Head.GetComponent<PhysicalBehaviour>().RefreshOutline();
        }
    }

    public class NoPower : Power
    {
        public override void Start()
        {
            AttemptAddingToList = false;
            base.Start();
        }

        public static Power SetPower(PersonBehaviour Person, LimbBehaviour TargetLimb)
        {
            var power = TargetLimb.gameObject.AddComponent<NoPower>();
            return power;
        }

        public static Power SetPower(PersonBehaviour Person)
        {
            var power = Person.gameObject.AddComponent<NoPower>();
            return power;
        }
    }

    public enum TargettedLimb
    {
        Head,
        Body,
        FrontArm,
        BackArm,
        FrontLeg,
        BackLeg,
        Internal
    }

    public class Power : MonoBehaviour
    {
        public bool Enabled = false;
        public GameObject button = null;
        public TargettedLimb targetLimb;
        public Sprite icon;
        public string Name;
        public string Description;
        public bool AttemptAddingToList = true;

        public virtual void Start()
        {
            if (AttemptAddingToList == false)
                return;
            
            if (targetLimb == TargettedLimb.Head)
            {
                if (TryGetComponent<LimbBehaviour>(out var limb))
                {
                    if (!limb.Person.Limbs[0].GetComponent<PowerMenu>())
                    {
                        var pmenu = limb.Person.Limbs[0].gameObject.AddComponent<PowerMenu>();
                        pmenu.CreateUI();
                        pmenu.AddButton(Name, Description, icon, this);
                    }
                    else
                    {
                        var pmenu = limb.Person.Limbs[0].gameObject.GetComponent<PowerMenu>();
                        pmenu.AddButton(Name, Description, icon, this);
                    }
                }
                else if (TryGetComponent<PersonBehaviour>(out var person))
                {
                    if (!person.Limbs[0].GetComponent<PowerMenu>())
                    {
                        var pmenu = person.Limbs[0].gameObject.AddComponent<PowerMenu>();
                        pmenu.CreateUI();
                        pmenu.AddButton(Name, Description, icon, this);
                    }
                    else
                    {
                        var pmenu = person.Limbs[0].gameObject.GetComponent<PowerMenu>();
                        pmenu.AddButton(Name, Description, icon, this);
                    }
                }
            }
            else if (targetLimb == TargettedLimb.Body)
            {
                if (TryGetComponent<LimbBehaviour>(out var limb))
                {
                    if (!limb.Person.Limbs[1].GetComponent<PowerMenu>())
                    {
                        var pmenu = limb.Person.Limbs[1].gameObject.AddComponent<PowerMenu>();
                        pmenu.CreateUI();
                        pmenu.AddButton(Name, Description, icon, this);
                    }
                    else
                    {
                        var pmenu = limb.Person.Limbs[1].gameObject.GetComponent<PowerMenu>();
                        pmenu.AddButton(Name, Description, icon, this);
                    }
                }
                else if (TryGetComponent<PersonBehaviour>(out var person))
                {
                    if (!person.Limbs[1].GetComponent<PowerMenu>())
                    {
                        var pmenu = person.Limbs[1].gameObject.AddComponent<PowerMenu>();
                        pmenu.CreateUI();
                        pmenu.AddButton(Name, Description, icon, this);
                    }
                    else
                    {
                        var pmenu = person.Limbs[1].gameObject.GetComponent<PowerMenu>();
                        pmenu.AddButton(Name, Description, icon, this);
                    }
                }
            }
            else if (targetLimb == TargettedLimb.FrontArm)
            {
                if (TryGetComponent<LimbBehaviour>(out var limb))
                {
                    if (!limb.Person.Limbs[11].GetComponent<PowerMenu>())
                    {
                        var pmenu = limb.Person.Limbs[11].gameObject.AddComponent<PowerMenu>();
                        pmenu.CreateUI();
                        pmenu.AddButton(Name, Description, icon, this);
                    }
                    else
                    {
                        var pmenu = limb.Person.Limbs[11].gameObject.GetComponent<PowerMenu>();
                        pmenu.AddButton(Name, Description, icon, this);
                    }
                }
                else if (TryGetComponent<PersonBehaviour>(out var person))
                {
                    if (!person.Limbs[11].GetComponent<PowerMenu>())
                    {
                        var pmenu = person.Limbs[11].gameObject.AddComponent<PowerMenu>();
                        pmenu.CreateUI();
                        pmenu.AddButton(Name, Description, icon, this);
                    }
                    else
                    {
                        var pmenu = person.Limbs[11].gameObject.GetComponent<PowerMenu>();
                        pmenu.AddButton(Name, Description, icon, this);
                    }
                }
            }
            else if (targetLimb == TargettedLimb.BackArm)
            {
                if (TryGetComponent<LimbBehaviour>(out var limb))
                {
                    if (!limb.Person.Limbs[13].GetComponent<PowerMenu>())
                    {
                        var pmenu = limb.Person.Limbs[13].gameObject.AddComponent<PowerMenu>();
                        pmenu.CreateUI();
                        pmenu.AddButton(Name, Description, icon, this);
                    }
                    else
                    {
                        var pmenu = limb.Person.Limbs[13].gameObject.GetComponent<PowerMenu>();
                        pmenu.AddButton(Name, Description, icon, this);
                    }
                }
                else if (TryGetComponent<PersonBehaviour>(out var person))
                {
                    if (!person.Limbs[13].GetComponent<PowerMenu>())
                    {
                        var pmenu = person.Limbs[13].gameObject.AddComponent<PowerMenu>();
                        pmenu.CreateUI();
                        pmenu.AddButton(Name, Description, icon, this);
                    }
                    else
                    {
                        var pmenu = person.Limbs[13].gameObject.GetComponent<PowerMenu>();
                        pmenu.AddButton(Name, Description, icon, this);
                    }
                }
            }
            else if (targetLimb == TargettedLimb.FrontLeg)
            {
                if (TryGetComponent<LimbBehaviour>(out var limb))
                {
                    Debug.Log("HAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                    if (!limb.Person.Limbs[5].GetComponent<PowerMenu>())
                    {
                        var pmenu = limb.Person.Limbs[5].gameObject.AddComponent<PowerMenu>();
                        pmenu.CreateUI();
                        pmenu.AddButton(Name, Description, icon, this);
                    }
                    else
                    {
                        var pmenu = limb.Person.Limbs[5].gameObject.GetComponent<PowerMenu>();
                        pmenu.AddButton(Name, Description, icon, this);
                    }
                }
                else if (TryGetComponent<PersonBehaviour>(out var person))
                {
                    if (!person.Limbs[5].GetComponent<PowerMenu>())
                    {
                        var pmenu = person.Limbs[5].gameObject.AddComponent<PowerMenu>();
                        pmenu.CreateUI();
                        pmenu.AddButton(Name, Description, icon, this);
                    }
                    else
                    {
                        var pmenu = person.Limbs[5].gameObject.GetComponent<PowerMenu>();
                        pmenu.AddButton(Name, Description, icon, this);
                    }
                }
            }
            else if (targetLimb == TargettedLimb.BackLeg)
            {
                if (TryGetComponent<LimbBehaviour>(out var limb))
                {
                    Debug.Log("HAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                    if (!limb.Person.Limbs[8].GetComponent<PowerMenu>())
                    {
                        var pmenu = limb.Person.Limbs[8].gameObject.AddComponent<PowerMenu>();
                        pmenu.CreateUI();
                        pmenu.AddButton(Name, Description, icon, this);
                    }
                    else
                    {
                        var pmenu = limb.Person.Limbs[8].gameObject.GetComponent<PowerMenu>();
                        pmenu.AddButton(Name, Description, icon, this);
                    }
                }
                else if (TryGetComponent<PersonBehaviour>(out var person))
                {
                    if (!person.Limbs[8].GetComponent<PowerMenu>())
                    {
                        var pmenu = person.Limbs[8].gameObject.AddComponent<PowerMenu>();
                        pmenu.CreateUI();
                        pmenu.AddButton(Name, Description, icon, this);
                    }
                    else
                    {
                        var pmenu = person.Limbs[8].gameObject.GetComponent<PowerMenu>();
                        pmenu.AddButton(Name, Description, icon, this);
                    }
                }
            }
            else if (targetLimb == TargettedLimb.Internal)
            {
                if (TryGetComponent<PersonBehaviour>(out var person))
                {
                    if (!person.GetComponent<InternalPowerMenu>())
                    {
                        var pmenu = person.gameObject.AddComponent<InternalPowerMenu>();
                        pmenu.CreateUI();
                        pmenu.AddButton(Name, Description, icon, this);
                    }
                    else
                    {
                        var pmenu = person.gameObject.GetComponent<InternalPowerMenu>();
                        pmenu.AddButton(Name, Description, icon, this);
                    }
                }
            }
        }

        public virtual void EnablePower()
        {
            Enabled = true;
            button?.transform.GetChild(0)?.gameObject.SetActive(false);
        }

        public virtual void DisablePower()
        {
            Enabled = false;
            button?.transform.GetChild(0)?.gameObject.SetActive(true);
        }
    }

    public class AbilityMenus : MonoBehaviour
    {
        public GameObject ui;
        public Transform Contents;
        public static GameObject MenuPrefab;

        public PowerMenu Head;
        public PowerMenu Body;
        public PowerMenu FrontArm;
        public PowerMenu BackArm;
        public PowerMenu FrontLeg;
        public PowerMenu BackLeg;
        public InternalPowerMenu Internal;
        public GameObject contextMenu;

        public void Update()
        {
            if (!contextMenu)
                contextMenu = GameObject.Find("Scrolling context menu");

            if (contextMenu)
                if(contextMenu.activeInHierarchy)
                    foreach (var text in contextMenu.transform.GetChild(0).GetChild(0).GetComponentsInChildren<TextMeshProUGUI>())
                        if (text.text == "Ability Menus")
                            text.transform.parent.SetSiblingIndex(0);
        }

        public void Start()
        {
            CreateUI();
            contextMenu = GameObject.Find("Scrolling context menu");
            foreach (var limb in transform.root.GetComponent<PersonBehaviour>().Limbs)
            {
                limb.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("Ability Menus", "Ability Menus", "Ability Menus", () =>
                {
                    UpdateMenu();
                    ui.SetActive(true);
                }));

                var contextMenu = limb.GetComponent<PhysicalBehaviour>().ContextMenuOptions;
                int changeSkinIndex = contextMenu.Buttons.FindIndex(b => b.Identity == "Ability Menus");
                if (changeSkinIndex > 0)
                {
                    var changeSkinButton = contextMenu.Buttons[changeSkinIndex];
                    contextMenu.Buttons.RemoveAt(changeSkinIndex);
                    contextMenu.Buttons.Insert(0, changeSkinButton);
                }
            }
        }

        public void UpdateMenu()
        {
            var person = transform.root.GetComponent<PersonBehaviour>();

            if (person.Limbs[0].TryGetComponent<PowerMenu>(out var power))
                Head = power;
            if (person.Limbs[1].TryGetComponent<PowerMenu>(out var power2))
                Body = power2;
            if (person.Limbs[11].TryGetComponent<PowerMenu>(out var power3))
                FrontArm = power3;
            if (person.Limbs[13].TryGetComponent<PowerMenu>(out var power4))
                BackArm = power4;
            if (person.Limbs[5].TryGetComponent<PowerMenu>(out var power5))
                FrontLeg = power5;
            if (person.Limbs[8].TryGetComponent<PowerMenu>(out var power6))
                BackLeg = power6;
            if (person.TryGetComponent<InternalPowerMenu>(out var power7))
                Internal = power7;

            Contents.FindChild("Head").gameObject.SetActive(Head);
            Contents.FindChild("Body").gameObject.SetActive(Body);
            Contents.FindChild("FrontArm").gameObject.SetActive(FrontArm);
            Contents.FindChild("BackArm").gameObject.SetActive(BackArm);
            Contents.FindChild("FrontLeg").gameObject.SetActive(FrontLeg);
            Contents.FindChild("BackLeg").gameObject.SetActive(BackLeg);
            Contents.FindChild("Internal").gameObject.SetActive(Internal);
        }

        public void CreateUI()
        {
            if (ui != null)
            {
                Destroy(ui);
            }
            GameObject canvas = GameObject.Find("Canvas");
            ui = Instantiate(MenuPrefab);
            ui.transform.SetParent(canvas.transform);
            ui.transform.SetSiblingIndex(1);
            ui.transform.localScale = Vector3.one * 1.3f;
            ui.GetComponent<RectTransform>().sizeDelta = new Vector2(720, 870f);
            ui.GetComponent<RectTransform>().localPosition = Vector2.zero;
            Contents = ui.transform.GetChild(3).GetChild(0).GetChild(0);

            GameObject button = ui.transform.FindChild("Button").gameObject;

            Button uiButton = button.GetComponent<Button>();
            uiButton.onClick.AddListener(() => ui.SetActive(false));
            ui.SetActive(false);

            Contents.FindChild("Head").GetComponent<Button>().onClick.AddListener(() =>
            {
                if (!Head)
                    return;
                Head.ui.SetActive(true);
                ui.SetActive(false);
            });

            Contents.FindChild("Body").GetComponent<Button>().onClick.AddListener(() =>
            {
                if (!Body)
                    return;
                Body.ui.SetActive(true);
                ui.SetActive(false);
            });

            Contents.FindChild("FrontArm").GetComponent<Button>().onClick.AddListener(() =>
            {
                if (!FrontArm)
                    return;
                FrontArm.ui.SetActive(true);
                ui.SetActive(false);
            });

            Contents.FindChild("BackArm").GetComponent<Button>().onClick.AddListener(() =>
            {
                if (!BackArm)
                    return;
                BackArm.ui.SetActive(true);
                ui.SetActive(false);
            });

            Contents.FindChild("FrontLeg").GetComponent<Button>().onClick.AddListener(() =>
            {
                if (!FrontLeg)
                    return;
                FrontLeg.ui.SetActive(true);
                ui.SetActive(false);
            });

            Contents.FindChild("BackLeg").GetComponent<Button>().onClick.AddListener(() =>
            {
                if (!BackLeg)
                    return;
                BackLeg.ui.SetActive(true);
                ui.SetActive(false);
            });

            Contents.FindChild("Internal").GetComponent<Button>().onClick.AddListener(() =>
            {
                if (!Internal)
                    return;
                Internal.ui.SetActive(true);
                ui.SetActive(false);
            });

            ui.SetActive(false);
        }
    }

    public class InternalPowerMenu : MonoBehaviour
    {
        public static Sprite Background = Mod.PBackground;
        public static Sprite closeSpriteButton = Mod.closeSpriteButton;
        public static Sprite ToggledSprite = Mod.ToggledSprite;
        public GameObject ui;
        public static GameObject MenuPrefab;
        private int buttonCount = 0;
        private List<(GameObject, Power)> Buttons = new List<(GameObject, Power)>();
        public static Sprite buttonSprite = Mod.buttonSprite;
        public TMPro.TextMeshProUGUI Selnametext;
        public TMPro.TextMeshProUGUI Seldesctext;
        public Power CurrentPower;
        public GameObject currentButton;

        public void Start()
        {
            CreateUI();

            if(!transform.root.GetComponent<AbilityMenus>())
                transform.root.gameObject.AddComponent<AbilityMenus>();

            for (int i = 0; i < Buttons.Count; i++)
            {
                Buttons[i].Item1.transform.SetParent(ui.transform.FindChild("Scroll View").FindChild("Viewport").FindChild("Content"));
                Buttons[i].Item1.transform.localScale = Vector3.one;
            }
        }

        public void OnEnable()
        {
            for (int i = 0; i < Buttons.Count; i++)
            {
                Buttons[i].Item1.transform.SetParent(ui.transform.FindChild("Scroll View").FindChild("Viewport").FindChild("Content"));
                Buttons[i].Item1.transform.localScale = Vector3.one;
            }
        }

        public void CreateUI()
        {
            if (ui != null)
            {
                Destroy(ui);
            }
            GameObject canvas = GameObject.Find("Canvas");
            ui = Instantiate(MenuPrefab);
            ui.transform.SetParent(canvas.transform);
            ui.transform.SetSiblingIndex(1);
            ui.transform.localScale = Vector3.one * 1.3f;
            ui.GetComponent<RectTransform>().sizeDelta = new Vector2(1000, 870f);
            ui.GetComponent<RectTransform>().localPosition = Vector2.zero;

            GameObject selectedTextureButtonObject = ui.transform.FindChild("Toggle").gameObject;

            Button acceptUIButton = selectedTextureButtonObject.GetComponent<Button>();
            acceptUIButton.onClick.AddListener(() => ChangePower(CurrentPower));

            GameObject Selname = ui.transform.FindChild("AbilityNameContainer").GetChild(0).gameObject;
            GameObject Seldesc = ui.transform.FindChild("DescriptionContainer").GetChild(0).gameObject;


            Selnametext = Selname.GetComponent<TMPro.TextMeshProUGUI>();
            Seldesctext = Seldesc.GetComponent<TMPro.TextMeshProUGUI>();

            GameObject button = ui.transform.FindChild("Button").gameObject;

            Button uiButton = button.GetComponent<Button>();
            uiButton.onClick.AddListener(() => ui.SetActive(false));

            GameObject back = ui.transform.FindChild("Back").gameObject;

            Button uiBack = back.GetComponent<Button>();
            uiBack.onClick.AddListener(() =>
            {
                if (!transform.root.TryGetComponent<AbilityMenus>(out var ab))
                    return;
                ab.UpdateMenu();
                ab.ui.SetActive(true);
                ui.SetActive(false);
            });

            ui.SetActive(false);
        }

        public void AddButton(string name, string description, Sprite sprite, Power power)
        {
            if (ui != null)
            {
                GameObject canvas = GameObject.Find("Canvas");
                GameObject button = new GameObject(name);
                Image buttonImage = button.AddComponent<Image>();
                buttonImage.sprite = sprite;

                RectTransform buttonTransform = button.GetComponent<RectTransform>();

                Button uiButton = button.AddComponent<Button>();
                uiButton.onClick.AddListener(() =>
                {
                    CurrentPower = power;
                    currentButton = button;
                    Selnametext.text = "<align=center>" + name + "</align>";
                    Seldesctext.text = description;
                });
                power.button = button;
                Buttons.Add((button, power));
                button.transform.SetParent(ui.transform.FindChild("Scroll View").FindChild("Viewport").FindChild("Content"));
                button.transform.localScale = Vector3.one;
                GameObject offThing = new GameObject("OffThing");
                offThing.transform.parent = buttonTransform.transform;
                var offRect = offThing.AddComponent<RectTransform>();
                offRect.anchorMin = Vector2.zero;
                offRect.anchorMax = Vector2.one;
                offRect.offsetMin = Vector2.zero;
                offRect.offsetMax = Vector2.zero;
                offThing.AddComponent<Image>().sprite = ToggledSprite;
                offThing.GetComponent<RectTransform>().localScale = Vector3.one;
                offThing.GetComponent<RectTransform>().localPosition = Vector3.zero;
                offThing.SetActive(false);
                buttonCount++;
            }
        }

        void ChangePower(Power power)
        {
            if (!power.Enabled)
            {
                power.EnablePower();
                currentButton.transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                power.DisablePower();
                currentButton.transform.GetChild(0).gameObject.SetActive(true);
            }
        }

        private void OnDestroy()
        {
            if (ui != null)
            {
                Destroy(ui);
            }
        }

    }

    public class PowerMenu : MonoBehaviour
    {
        public static Sprite Background = Mod.PBackground;
        public static Sprite closeSpriteButton = Mod.closeSpriteButton;
        public GameObject ui;
        public static GameObject MenuPrefab;
        private int buttonCount = 0;
        private List<GameObject> Buttons = new List<GameObject>();
        public static Sprite buttonSprite = Mod.buttonSprite;
        public TMPro.TextMeshProUGUI Selnametext;
        public TMPro.TextMeshProUGUI Seldesctext;
        public Power CurrentPower;

        public void Start()
        {
            CreateUI();

            if (!transform.root.GetComponent<AbilityMenus>())
                transform.root.gameObject.AddComponent<AbilityMenus>();

            for (int i = 0; i < Buttons.Count; i++)
            {
                Buttons[i].transform.SetParent(ui.transform.FindChild("Scroll View").FindChild("Viewport").FindChild("Content"));
                Buttons[i].transform.localScale = Vector3.one;
            }
        }

        public void OnEnable()
        {
            for (int i = 0; i < Buttons.Count; i++)
            {
                Buttons[i].transform.SetParent(ui.transform.FindChild("Scroll View").FindChild("Viewport").FindChild("Content"));
                Buttons[i].transform.localScale = Vector3.one;
            }
        }

        public void CreateUI()
        {
            if (ui != null)
            {
                Destroy(ui);
            }
            GameObject canvas = GameObject.Find("Canvas");
            ui = Instantiate(MenuPrefab);
            ui.transform.SetParent(canvas.transform);
            ui.transform.SetSiblingIndex(1);
            ui.transform.localScale = Vector3.one * 1.3f;
            ui.GetComponent<RectTransform>().sizeDelta = new Vector2(1000, 870f);
            ui.GetComponent<RectTransform>().localPosition = Vector2.zero;

            GameObject selectedTextureButtonObject = ui.transform.FindChild("Apply").gameObject;

            Button acceptUIButton = selectedTextureButtonObject.GetComponent<Button>();
            acceptUIButton.onClick.AddListener(() => ChangePower(CurrentPower));

            GameObject Selname = ui.transform.FindChild("AbilityNameContainer").GetChild(0).gameObject;
            GameObject Seldesc = ui.transform.FindChild("DescriptionContainer").GetChild(0).gameObject;


            Selnametext = Selname.GetComponent<TMPro.TextMeshProUGUI>();
            Seldesctext = Seldesc.GetComponent<TMPro.TextMeshProUGUI>();

            GameObject button = ui.transform.FindChild("Button").gameObject;

            Button uiButton = button.GetComponent<Button>();
            uiButton.onClick.AddListener(() => ui.SetActive(false));
            ui.SetActive(false);

            GameObject back = ui.transform.FindChild("Back").gameObject;

            Button uiBack = back.GetComponent<Button>();
            uiBack.onClick.AddListener(() =>
            {
                if (!transform.root.TryGetComponent<AbilityMenus>(out var ab))
                    return;
                ab.UpdateMenu();
                ab.ui.SetActive(true);
                ui.SetActive(false);
            });

            ui.SetActive(false);

            if (!GetComponent<NoPower>())
                AddButton(" No Power", "Choose this option to have no power enabled on this limb.", Mod.NoPowerSprite, NoPower.SetPower(gameObject.GetComponent<LimbBehaviour>().Person, gameObject.GetComponent<LimbBehaviour>()));
        }

        public void AddButton(string name, string description, Sprite sprite, Power power)
        {
            if (ui != null)
            {
                GameObject canvas = GameObject.Find("Canvas");
                GameObject button = new GameObject(name);
                Image buttonImage = button.AddComponent<Image>();
                buttonImage.sprite = sprite;

                RectTransform buttonTransform = button.GetComponent<RectTransform>();

                Button uiButton = button.AddComponent<Button>();
                uiButton.onClick.AddListener(() =>
                {
                    CurrentPower = power;
                    Selnametext.text = "<align=center>" + name + "</align>";
                    Seldesctext.text = description;
                });

                Buttons.Add(button);
                button.transform.SetParent(ui.transform.FindChild("Scroll View").FindChild("Viewport").FindChild("Content"));
                button.transform.localScale = Vector3.one;
                buttonCount++;
            }
        }

        void ChangePower(Power power)
        {
            foreach (Power power2 in GetComponents<Power>())
            {
                if (power2 != power)
                    power2.DisablePower();
            }

            if (!power.Enabled)
            {
                power.EnablePower();
            }

            if (ui != null)
            {
                ui.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            if (ui != null)
            {
                Destroy(ui);
            }
        }

    }

    public class TextureMenu : MonoBehaviour
    {
        public GameObject ui;
        private int buttonCount = 0;
        public List<GameObject> Buttons = new List<GameObject>();
        public static GameObject MenuPrefab;
        public TMPro.TextMeshProUGUI Selnametext;
        public List<Sprite> SelectedSkin;
        public GameObject PreviewHuman;
        public UnityEvent SelectedEvent;
        public UnityEvent DeselectedEvent;
        public UnityEvent PreviousDeselectedEvent;
        public Sprite SelectedCapeCollar;
        public Sprite SelectedCape;
        private PersonBehaviour Person;
        public GameObject contextMenu;

        public void Update()
        {
            if(!contextMenu)
                contextMenu = GameObject.Find("Scrolling context menu");

            if (contextMenu)
                if (contextMenu.activeInHierarchy)
                    foreach (var text in contextMenu.transform.GetChild(0).GetChild(0).GetComponentsInChildren<TextMeshProUGUI>())
                        if (text.text == "Change Skin")
                            text.transform.parent.SetSiblingIndex(0);
        }

        public void Start()
        {
            Person = GetComponent<PersonBehaviour>();
            CreateUI();
            contextMenu = GameObject.Find("Scrolling context menu");
            foreach (var limb in Person.Limbs)
            {
                limb.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("Change Skin", "Change Skin", "Change Skin", () =>
                {
                    ui.SetActive(true);
                }));

                var contextMenu = limb.GetComponent<PhysicalBehaviour>().ContextMenuOptions;
                int changeSkinIndex = contextMenu.Buttons.FindIndex(b => b.Identity == "Change Skin");
                if (changeSkinIndex > 0)
                {
                    var changeSkinButton = contextMenu.Buttons[changeSkinIndex];
                    contextMenu.Buttons.RemoveAt(changeSkinIndex);
                    contextMenu.Buttons.Insert(0, changeSkinButton);
                }
            }

            for (int i = 0; i < Buttons.Count; i++)
            {
                Buttons[i].transform.SetParent(ui.transform.GetChild(0).FindChild("Scroll View").FindChild("Viewport").FindChild("Content"));
                Buttons[i].transform.localScale = Vector3.one;
            }
        }

        public void CreateUI()
        {
            if (ui != null)
            {
                Destroy(ui);
            }
            GameObject canvas = GameObject.Find("Canvas");
            ui = Instantiate(MenuPrefab);
            ui.transform.SetParent(canvas.transform);
            ui.transform.SetSiblingIndex(1);
            ui.transform.localScale = Vector3.one * 1.3f;
            ui.GetComponent<RectTransform>().sizeDelta = new Vector2(1220, 870f);
            ui.GetComponent<RectTransform>().localPosition = Vector2.zero;
            var ui2 = ui.transform.GetChild(0);
            PreviewHuman = ui.transform.GetChild(1).GetChild(1).gameObject;
            GameObject selectedTextureButtonObject = ui2.FindChild("SelectedTextureButton").gameObject;

            GameObject Selname = ui2.FindChild("SelectedTextureName").gameObject;

            Selnametext = Selname.GetComponent<TMPro.TextMeshProUGUI>();

            Button acceptUIButton = selectedTextureButtonObject.GetComponent<Button>();
            acceptUIButton.onClick.AddListener(() => ChangeTexture(SelectedSkin));

            GameObject button = ui2.FindChild("Button").gameObject;

            ScrollRect scrollview = ui2.FindChild("Scroll View").GetComponent<ScrollRect>();

            Button uiButton = button.GetComponent<Button>();
            uiButton.onClick.AddListener(() => ui.SetActive(false));
            ui.SetActive(false);
        }

        public void AddButton(string name, Sprite sprite, List<Sprite> limbs, UnityEvent OnAddEvent = null, UnityEvent OnRemoveEvent = null, Sprite Cape = null, Sprite CapeCollar = null)
        {
            if (ui != null)
            {
                GameObject canvas = GameObject.Find("Canvas");
                GameObject button = new GameObject(name);
                Image buttonImage = button.AddComponent<Image>();
                buttonImage.sprite = sprite;

                RectTransform buttonTransform = button.GetComponent<RectTransform>();

                Button uiButton = button.AddComponent<Button>();
                uiButton.onClick.AddListener(() =>
                {
                    SelectedSkin = limbs;
                    SelectedCape = Cape;
                    SelectedCapeCollar = CapeCollar;
                    SelectedEvent = OnAddEvent;
                    DeselectedEvent = OnRemoveEvent;
                    Selnametext.text = name;

                    foreach (var ci in PreviewHuman.GetComponentsInChildren<Image>())
                    {
                        ci.sprite = null;
                        ci.color = new Color(0, 0, 0, 0);

                        foreach (var limb in SelectedSkin)
                        {
                            if (ci.name.Contains(limb.name))
                                ci.sprite = limb;
                        }

                        ci.SetNativeSize();
                        if (ci.GetComponent<Image>().sprite != null)
                            ci.GetComponent<Image>().color = Color.white;
                    }

                    PreviewHuman.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = CapeCollar;
                    PreviewHuman.transform.GetChild(1).GetChild(0).GetComponent<Image>().SetNativeSize();
                    PreviewHuman.transform.GetChild(1).GetChild(1).GetComponent<Image>().sprite = Cape;
                    PreviewHuman.transform.GetChild(1).GetChild(0).GetComponent<Image>().SetNativeSize();
                    PreviewHuman.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = CapeCollar ? Color.white : new Color(0, 0, 0, 0);
                    PreviewHuman.transform.GetChild(1).GetChild(1).GetComponent<Image>().color = Cape ? Color.white : new Color(0, 0, 0, 0);
                });

                Buttons.Add(button);
                buttonCount++;
            }
        }

        public void ChangeTexture(List<Sprite> Skin)
        {
            Person = transform.root.GetComponent<PersonBehaviour>();
            Person.gameObject.SendMessage("SkinChanged");
            PreviousDeselectedEvent?.Invoke();
            SelectedEvent?.Invoke();
            PreviousDeselectedEvent = DeselectedEvent;
            Timtam.MakeCustomSkin(Person, Skin, false, true);
            if (Person.Limbs[1].GetComponent<Cape>())
            {
                Cape cape = Person.Limbs[1].GetComponent<Cape>();
                cape.lineRenderer.SetColors(Color.white, Color.white);
                if (SelectedCape == null)
                {
                    cape.capeTexture = null;
                    cape.lineRenderer.SetColors(new Color(0, 0, 0, 0), new Color(0, 0, 0, 0));
                    cape.CapeCollar = null;
                }
                else
                {
                    cape.capeTexture = SelectedCape.texture;
                    cape.CapeCollar = SelectedCapeCollar;
                }

            }
            else
            {
                if (SelectedCape != null || SelectedCapeCollar != null)
                {
                    Cape.CreateCapeForPerson(Person, SelectedCape.texture, SelectedCapeCollar);
                }
            }
            if (ui != null)
            {
                ui.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            if (ui != null)
            {
                Destroy(ui);
            }
        }
    }

    public static class Functions{
        public static void CreatePicker<T>(UnityAction<T> PickedFunction) where T : Component
        {
            GameObject Picker = new GameObject("Picker");
            Picker PickerComponent = Picker.AddComponent<Picker>();
            PickerComponent.Upds(PickedFunction);
        }
        public static Texture2D Clone(Texture2D Tex){
            RenderTexture rt = RenderTexture.GetTemporary(Tex.width, Tex.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.sRGB);
            Graphics.Blit(Tex, rt);
            RenderTexture prev = RenderTexture.active;
            RenderTexture.active = rt;
            Texture2D clone = new Texture2D(Tex.width, Tex.height, TextureFormat.RGBA32, false) { filterMode = FilterMode.Point };
            clone.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            clone.Apply();
            RenderTexture.active = prev;
            RenderTexture.ReleaseTemporary(rt);
            return clone;
        }
        public static Sprite Clone(Sprite spr)
        {
            return Sprite.Create(Clone(spr.texture), new Rect(spr.textureRect.position-spr.textureRectOffset, spr.rect.size), Vector2.one * .5f, spr.pixelsPerUnit, 0U, SpriteMeshType.FullRect, spr.border, false);
        }
        public static Sprite Clone(Sprite spr, Texture2D tex){
            return Sprite.Create(tex, new Rect(spr.textureRect.position-spr.textureRectOffset, spr.rect.size), Vector2.one*.5f, spr.pixelsPerUnit, 0U, SpriteMeshType.FullRect, spr.border, false);
        }
        public static int GetArea(Texture2D Tex, Rect rect = default){
            int res = 0;
            if (rect != default){
                foreach (Color i in Tex.GetPixels((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height)){
                    if (i.a > 0) ++res;
                }
            }
            else{
                foreach (Color i in Tex.GetPixels()){
                    if (i.a > 0) ++res;
                }
            }
            return res;
        }
    }

    public class MrNegative : Power{
        public PersonBehaviour Person;
        public static float GrayStrength = .9f;
        public static int PixelPerTurn = 2;
        public static float TurnDuration = .001f;
        public static float ContrastStrength = 1.5f;
        public static float StrengthDark = 2f;
        public static float StrengthLight = 1.2f;
        public static float Midpoint = .95f;
        public static Dictionary<string, int> LimbNameIndex = new Dictionary<string, int>(){
            {"Head", 0},
            {"UpperBody",1},
            {"MiddleBody",2},
            {"LowerBody",3},
            {"UpperLegFront",4},
            {"LowerLegFront",5},
            {"FootFront",6},
            {"UpperLeg",7},
            {"LowerLeg",8},
            {"Foot",9},
            {"UpperArmFront",10},
            {"LowerArmFront",11},
            {"UpperArm",12},
            {"LowerArm",13}
        };
        public static Dictionary<int, int> Order = new Dictionary<int, int>(){
            {0,0},
            {1,1},
            {2,2},
            {3,3},
            {4,4},
            {5,5},
            {6,6},
            {7,4},
            {8,5},
            {9,6},
            {10,1},
            {11,2},
            {12,1},
            {13,2},
        };
        public static GameObject DevilEffect;
        public static GameObject DevilAura;
        public static GameObject DarknessPrefab;
        public static GameObject EnergyBlast;
        public static GameObject DevilExplosion;
        public static GameObject EnergyCharge;
        public Texture2D[] OriginalTexture = new Texture2D[14]; // If Transformed set to true on initial, manually assign this
        private bool Transformed = false;
        private Dictionary<int, bool> CurrentOrder;
        private readonly ParticleSystem[] Auras = new ParticleSystem[14];
        private readonly GameObject[] Darknesses = new GameObject[14];
        private bool SkinChangedMid = false;
        public static Power SetPower(PersonBehaviour Person, Sprite Icon){
            MrNegative neg = Person.gameObject.AddComponent<MrNegative>();
            neg.Person = Person;
            Power pow = neg;
            pow.Name = "Negative Form";
            pow.Description = "Transforms the user into a negative version of themself.<color=\"yellow\">\n This power allows the user to transform their limbs into a negative state, altering their appearance and functionality. You can also revert the user to their original form by disabling the power.";
            pow.icon = Icon;
            pow.targetLimb = TargettedLimb.Internal;
            return pow;
        }
        public override void Start(){
            base.Start();

            for (int i=0; i<14; ++i){
                if (!Transformed)OriginalTexture[i] = Functions.Clone(Person.Limbs[i].SkinMaterialHandler.renderer.sprite.texture);
                Person.Limbs[i].SkinMaterialHandler.renderer.sprite = Functions.Clone(Person.Limbs[i].SkinMaterialHandler.renderer.sprite);

                ParticleSystem Aura = Instantiate(DevilAura, Person.Limbs[i].transform).GetComponent<ParticleSystem>();
                Aura.Stop();

                GameObject Darkness = Instantiate(DarknessPrefab, Person.Limbs[i].transform);
                Darkness.transform.localScale /= 30f;
                Darkness.GetComponent<SpriteRenderer>().color = new Color(1,1,0,.03f);

                Auras[i] = Aura;
                Darknesses[i] = Darkness;
            }
        }
        public override void EnablePower(){
            base.EnablePower();
            SkinChangedMid = false;
            CurrentOrder = new Dictionary<int, bool>();
            for (int i = 0; i <= 7; ++i){
                CurrentOrder.Add(i, false);
            }
            CurrentOrder[0] = true;
            for (int i = 0; i < Person.Limbs.Length; ++i){
                StartCoroutine(TransformFormLimb(Person.Limbs[i].SkinMaterialHandler.renderer.sprite.texture, Person.Limbs[i], i, i<14?Order[i]:0));
            }
        }
        public override void DisablePower(){
            base.DisablePower();
            CurrentOrder = new Dictionary<int, bool>();
            for (int i = 0; i <= 7; ++i){
                CurrentOrder.Add(i, false);
            }
            CurrentOrder[0] = true;
            for (int i = 0; i < Person.Limbs.Length; ++i){
                StartCoroutine(TransformBackLimb(Person.Limbs[i].SkinMaterialHandler.renderer.sprite.texture, Person.Limbs[i], i, 6-(i<14?Order[i]:0)));
            }
        }
        public void SkinChanged() => SkinChangedMid = true;
        IEnumerator TransformFormLimb(Texture2D Tex, LimbBehaviour Limb, int index, int Order, bool reversed=false){
            yield return new WaitUntil(() => CurrentOrder[Order]);
            int count = 0;
            for (int y = Tex.height-1; y >=0; --y){
                for (int x = Tex.width-1; x >=0; --x){
                    Color clr = Tex.GetPixel(reversed?Tex.width-1-x:x, reversed?Tex.height-1-y:y);

                    if (clr.a==0) continue;

                    NegativeFilter(ref clr, true);

                    if (Limb==Limb.Person.Limbs[0] && x==25 && y==23) clr = Color.white*.9f;

                    Tex.SetPixel(reversed?Tex.width-1-x:x, reversed?Tex.height-1-y:y, clr);
                    ++count;
                    if (count == PixelPerTurn){
                        Tex.Apply();
                        count = 0;
                        yield return new WaitForSeconds(TurnDuration);
                    }
                }
            }
            CurrentOrder[Order+1]=true;
            Tex.Apply();
            Darknesses[index].SetActive(true);
            Auras[index].Play();
        }
        IEnumerator TransformBackLimb(Texture2D Tex, LimbBehaviour Limb, int index, int Order, bool reversed=true){
            yield return new WaitUntil(() => CurrentOrder[Order]);
            if (SkinChangedMid){
                yield return new WaitForSeconds(.5f);
            }else{
                int count = 0;
                for (int y = Tex.height-1; y >=0; --y){
                    for (int x = Tex.width-1; x >=0; --x){
                        Color clr = OriginalTexture[index].GetPixel(reversed?Tex.width-1-x:x, reversed?Tex.height-1-y:y);

                        if (clr.a==0) continue;

                        Tex.SetPixel(reversed?Tex.width-1-x:x, reversed?Tex.height-1-y:y, clr);
                        ++count;
                        if (count == PixelPerTurn){
                            Tex.Apply();
                            count = 0;
                            yield return new WaitForSeconds(TurnDuration);
                        }
                    }
                }
                Tex.Apply();
            }
            CurrentOrder[Order+1]=true;
            Darknesses[index].SetActive(false);
            Auras[index].Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
        public static void NegativeFilter(ref Color clr, bool FixShades=false, float Offset=1f){
            // Invert Color
            clr.r = 1 - clr.r;
            clr.g = 1 - clr.g;
            clr.b = 1 - clr.b;

            // Desaturate
            float gray = (clr.r + clr.g + clr.b) / 3f;
            clr.r = Mathf.Lerp(clr.r, gray, GrayStrength);
            clr.g = Mathf.Lerp(clr.g, gray, GrayStrength);
            clr.b = Mathf.Lerp(clr.b, gray, GrayStrength);

            // Contrast
            clr.r = Mathf.Clamp01((clr.r - 0.5f) * ContrastStrength + 0.5f);
            clr.g = Mathf.Clamp01((clr.g - 0.5f) * ContrastStrength + 0.5f);
            clr.b = Mathf.Clamp01((clr.b - 0.5f) * ContrastStrength + 0.5f);
                
            if (gray > Offset*.7f && FixShades){
                clr.r = 1-AdjustChannel(clr.r)+.7f;
                clr.g = 1-AdjustChannel(clr.g)+.7f;
                clr.b = 1-AdjustChannel(clr.b)+.7f;
            }
        }
        public static float AdjustChannel(float c){
            if (c < Midpoint){
                return Mathf.Clamp01(Midpoint * Mathf.Pow(c / Midpoint, StrengthDark));
            }
            else{
                return Mathf.Clamp01(Midpoint + (1 - Midpoint) * Mathf.Pow((c - Midpoint) / (1 - Midpoint), 1 / StrengthLight));
            }
        }
        public class EnergyBlastPow : Power{
            public static float DamagePerSecond = 100f;
            public LimbBehaviour Limb;
            private ParticleSystem Beam;
            private bool Activated = false;
            public override void Start(){
                base.Start();
                Beam = Instantiate(EnergyBlast, transform).GetComponent<ParticleSystem>();
                Beam.transform.localPosition = new Vector3(0f, -.4f, 0f);
                Beam.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
                Beam.gameObject.AddComponent<ParticleBehaviour>();
                Beam.Stop();
            }
            public static Power SetPower(LimbBehaviour Limb, Sprite Icon){
                EnergyBlastPow cor = Limb.gameObject.AddComponent<EnergyBlastPow>();
                cor.Limb = Limb;
                Power pow = cor;
                pow.Name = "Energy Blast";
                pow.icon = Icon;
                if (Limb.name.Contains("Front")) pow.targetLimb = TargettedLimb.FrontArm;
                else pow.targetLimb = TargettedLimb.BackArm;
                return pow;
            }
            public void Use(){
                if (!Enabled) return;
                Activated = !Activated;
                if (Activated) Beam.Play();
                else Beam.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
            public override void DisablePower(){
                base.DisablePower();
                if (Activated) Beam.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
            public void OnDestroy(){
                if (Activated) Beam.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                Destroy(Beam.gameObject, Beam.main.duration + Beam.main.startLifetime.constantMax);
            }
            private class ParticleBehaviour : MonoBehaviour{
                public void OnParticleCollision(GameObject other){
                    ParticleSystem Explosion =  Instantiate(DevilExplosion, other.transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
                    Explosion.transform.localScale /= 2f;
                    Explosion.transform.GetChild(0).localScale /= 2f;
                    Destroy(Explosion, Explosion.main.duration + Explosion.main.startLifetime.constantMax);
                    if (other.TryGetComponent(out LimbBehaviour Limb)){
                        Limb.Damage(DamagePerSecond * Time.deltaTime);
                    }
                }
            }
        }
        public class Corrupt : Power{
            public LimbBehaviour Limb;
            private GameObject Effect;
            private bool Activated = false;
            public static Power SetPower(LimbBehaviour Limb, Sprite Icon){
                Corrupt cor = Limb.gameObject.AddComponent<Corrupt>();
                cor.Limb = Limb;
                Power pow = cor;
                pow.Name = "Corruption";
                pow.icon = Icon;
                if (Limb.name.Contains("Front")) pow.targetLimb = TargettedLimb.FrontArm;
                else pow.targetLimb = TargettedLimb.BackArm;
                return pow;
            }
            public void Use(){
                if (!Enabled) return;
                Activated = !Activated;
                if (Activated) PlayEffect();
                else StopEffect();
            }

            private void PlayEffect(){
                Effect = Instantiate(DevilEffect);
                GameObject Darkness = Instantiate(DarknessPrefab, Effect.transform);
                Darkness.transform.localScale /= 20f;
                Darkness.GetComponent<SpriteRenderer>().color = new Color(1,1,0,.05f);

                MovementParent P = Effect.AddComponent<MovementParent>();
                P.Parent = transform;
                P.Offset = new Vector3(0f, -.2f, 0f);
            }

            private void StopEffect(){
                ParticleSystem PS = Effect.transform.GetChild(0).GetComponent<ParticleSystem>();
                PS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                Destroy(Effect, PS.main.duration + PS.main.startLifetime.constantMax);
            }
            public override void DisablePower(){
                base.DisablePower();
                if (Effect) StopEffect();
            }
            public void OnDestroy(){
                if (Effect){
                    StopEffect();
                }
            }

            public void OnCollisionEnter2D(Collision2D Collision){
                if (!Activated) return;
                if (Collision.gameObject.TryGetComponent(out LimbBehaviour Limb)){
                    if (Limb.Person.gameObject.HasComponent<Corrupted>()) return;
                    Limb.Person.gameObject.AddComponent<Corrupted>().Person = Limb.Person;
                    Activated = false;

                    MovementParent MP = Effect.GetComponent<MovementParent>();
                    MP.Parent = Collision.transform;
                    MP.Offset = Collision.transform.InverseTransformPoint(Collision.contacts[0].point);
                    StopEffect();

                    Debug.Log($"Corrupted {Limb.Person.gameObject.name}.");
                }else if (!Collision.transform.root.gameObject.HasComponent<InvertedWeapon>()){
                    Activated = false;
                    Collision.transform.root.gameObject.AddComponent<InvertedWeapon>();
                    MovementParent MP = Effect.GetComponent<MovementParent>();
                    MP.Parent = Collision.transform;
                    MP.Offset = Collision.transform.InverseTransformPoint(Collision.contacts[0].point);
                    StopEffect();
                }
            }
            public void FixedUpdate(){
                if (Activated && Limb.GripBehaviour.isHolding && !Limb.GripBehaviour.CurrentlyHolding.gameObject.HasComponent<InvertedWeapon>()){
                    Limb.GripBehaviour.CurrentlyHolding.gameObject.AddComponent<InvertedWeapon>();
                    StartCoroutine(Utils.DelayCoroutine(1, StopEffect));
                }
            }
        }
        public class Corrupted : MonoBehaviour{
            public PersonBehaviour Person;
            public static int Divisor = 110;
            private Texture2D[] OriginalTexture;
            private int Count;
            private Dictionary<int, bool> CurrentOrder;
            private bool SkinChangedMid = false;
            public void SkinChanged() => SkinChangedMid = true;
            public void Start(){
                OriginalTexture = new Texture2D[Person.Limbs.Length];
                for (int i = 0; i < Person.Limbs.Length; ++i){
                    OriginalTexture[i] = Person.Limbs[i].SkinMaterialHandler.renderer.sprite.texture;
                    Person.Limbs[i].SkinMaterialHandler.renderer.sprite = Functions.Clone(Person.Limbs[i].SkinMaterialHandler.renderer.sprite);
                }
                CurrentOrder = new Dictionary<int, bool>();
                for (int i = 0; i <= 7; ++i){
                    CurrentOrder.Add(i, false);
                }
                CurrentOrder[0] = true;
                for (int i = 0; i < Person.Limbs.Length; ++i){
                    StartCoroutine(TransformFormLimb(Person.Limbs[i].SkinMaterialHandler.renderer.sprite.texture, Person.Limbs[i], i<14?Order[i]:0));
                }
            }
            public IEnumerator Destroy(){
                Count = 0;
                CurrentOrder = new Dictionary<int, bool>();
                for (int i = 0; i <= 7; ++i){
                    CurrentOrder.Add(i, false);
                }
                CurrentOrder[0] = true;
                for (int i = 0; i < Person.Limbs.Length; ++i){
                    StartCoroutine(TransformBackLimb(Person.Limbs[i].SkinMaterialHandler.renderer.sprite.texture, Person.Limbs[i], i, 6-(i<14?Order[i]:0), true));
                }
                yield return new WaitUntil(()=>Count==Person.Limbs.Length);
                Destroy(this);
            }
            IEnumerator TransformFormLimb(Texture2D Tex, LimbBehaviour Limb, int Order, bool reversed=false){
                yield return new WaitUntil(() => CurrentOrder[Order]);
                foreach (SpriteRenderer SR in Limb.GetComponentsInChildren<SpriteRenderer>().Where(i=>i!=Limb.SkinMaterialHandler.renderer)){
                    StartCoroutine(TransformForm(SR));
                }
                int count = 0;
                Vector2 Size = Limb.SkinMaterialHandler.renderer.sprite.rect.size;
                Vector2 Point = Limb.SkinMaterialHandler.renderer.sprite.rect.position;
                float OverallSize = Functions.GetArea(Tex);
                for (int y = (int)(Point.y+Size.y-1); y >= (int)Point.y; --y){
                    for (int x = (int)(Point.x+Size.x-1); x >= (int)Point.x; --x){
                        Color clr = Tex.GetPixel(reversed?Tex.width-1-x:x, reversed?Tex.height-1-y:y);

                        if (clr.a==0) continue;

                        NegativeFilter(ref clr);

                        Tex.SetPixel(reversed?Tex.width-1-x:x, reversed?Tex.height-1-y:y, clr);
                        ++count;
                        if (count == (int)(PixelPerTurn*OverallSize/Divisor)){
                            Tex.Apply();
                            count = 0;
                            yield return new WaitForSeconds(TurnDuration);
                        }
                    }
                }
                CurrentOrder[Order+1]=true;
                Tex.Apply();
            }
            IEnumerator TransformForm(SpriteRenderer ChildSprite){
                int count = 0;
                ChildSprite.gameObject.AddComponent<CorruptedChildSprite>().OriginalTex=ChildSprite.sprite.texture;
                Texture2D Clone = Functions.Clone(ChildSprite.sprite.texture);
                ChildSprite.sprite = Functions.Clone(ChildSprite.sprite, Clone);
                int OverallSize = Functions.GetArea(Clone);
                for (int y = Clone.height-1; y >=0; --y){
                    for (int x = Clone.width-1; x >=0; --x){
                        Color clr = Clone.GetPixel(x,y);

                        if (clr.a==0) continue;

                        NegativeFilter(ref clr);

                        Clone.SetPixel(x,y, clr);
                        ++count;
                        if (count >= PixelPerTurn*OverallSize/Divisor){
                            Clone.Apply();
                            count = 0;
                            yield return new WaitForSeconds(TurnDuration);
                        }
                    }
                }
                Clone.Apply();
            }
            IEnumerator TransformBack(CorruptedChildSprite ChildSprite){
                int count = 0;
                Texture2D Orig = ChildSprite.OriginalTex;
                Texture2D Clone = Functions.Clone(ChildSprite.SR.sprite.texture);
                ChildSprite.SR.sprite = Functions.Clone(ChildSprite.SR.sprite, Clone);
                int OverallSize = Functions.GetArea(Clone);
                for (int y = Clone.height-1; y >=0; --y){
                    for (int x = Clone.width-1; x >=0; --x){
                        Color clr = Orig.GetPixel(x,y);

                        if (clr.a==0) continue;

                        Clone.SetPixel(x,y, clr);
                        ++count;
                        if (count == PixelPerTurn*OverallSize/Divisor){
                            Clone.Apply();
                            count = 0;
                            yield return new WaitForSeconds(TurnDuration);
                        }
                    }
                }
                ChildSprite.SR.sprite = Functions.Clone(ChildSprite.SR.sprite, Orig);
            }
            public class CorruptedChildSprite : MonoBehaviour{
                public Texture2D OriginalTex;
                public SpriteRenderer SR;
            }
            IEnumerator TransformBackLimb(Texture2D Tex, LimbBehaviour Limb, int index, int Order, bool reversed=true){
                yield return new WaitUntil(() => CurrentOrder[Order]);
                foreach (CorruptedChildSprite CS in Limb.GetComponentsInChildren<CorruptedChildSprite>()){
                    StartCoroutine(TransformBack(CS));
                }
                if (SkinChangedMid){
                    yield return new WaitForSeconds(.5f);
                }else{
                    int count = 0;
                    Vector2 Size = Limb.SkinMaterialHandler.renderer.sprite.rect.size;
                    Vector2 Point = Limb.SkinMaterialHandler.renderer.sprite.rect.position;
                    float OverallSize = Functions.GetArea(Tex);
                    for (int y = (int)(Point.y+Size.y-1); y >= (int)Point.y; --y){
                        for (int x = (int)(Point.x+Size.x-1); x >= (int)Point.x; --x){
                            Color clr = OriginalTexture[index].GetPixel(reversed?Tex.width-1-x:x, reversed?Tex.height-1-y:y);

                            if (clr.a==0) continue;

                            Tex.SetPixel(reversed?Tex.width-1-x:x, reversed?Tex.height-1-y:y, clr);
                            ++count;
                            if (count == PixelPerTurn*OverallSize/Divisor){
                                Tex.Apply();
                                count = 0;
                                yield return new WaitForSeconds(TurnDuration);
                            }
                        }
                    }
                    Tex.Apply();
                }
                CurrentOrder[Order+1]=true;
                ++Count;
            }
        }
        public class InvertedWeapon : MonoBehaviour{
            public void Start(){
                foreach (SpriteRenderer SR in gameObject.GetComponentsInChildren<SpriteRenderer>(true)){
                    if (SR.material.name.Contains("LightSprite")) continue;
                    SR.gameObject.AddComponent<Effect>().SR = SR;
                }
                GameObject EC = Instantiate(EnergyCharge, transform);
                foreach (ParticleSystem PS in EC.GetComponentsInChildren<ParticleSystem>()){
                    PS.gameObject.AddComponent<ChargedScript>().PS = PS;
                }
            }
            private class ChargedScript : MonoBehaviour{
                public ParticleSystem PS;
                public void Start(){
                    var Shape = PS.shape;
                    Shape.spriteRenderer = PS.transform.parent.parent.GetComponent<SpriteRenderer>();
                }
            }
            private class Effect : MonoBehaviour{
                public SpriteRenderer SR;
                public void Start(){
                    StartCoroutine(TransformForm(SR));
                }
                IEnumerator TransformForm(SpriteRenderer ChildSprite){
                    int count = 0;
                    Texture2D Clone = Functions.Clone(ChildSprite.sprite.texture);
                    Clone.name = "inverted shit";
                    Debug.Log(ChildSprite.sprite.pivot);
                    ChildSprite.sprite = Functions.Clone(ChildSprite.sprite, Clone);
                    Vector2 Size = ChildSprite.sprite.rect.size;
                    Vector2 Point = ChildSprite.sprite.textureRect.position;
                    float OverallSize = Functions.GetArea(Clone, ChildSprite.sprite.rect);
                    for (int y = (int)(Point.y+Size.y-1); y >= (int)Point.y; --y){
                        for (int x = (int)(Point.x+Size.x-1); x >= (int)Point.x; --x){
                            Color clr = Clone.GetPixel(x,y);

                            if (clr.a==0) continue;

                            NegativeFilter(ref clr);

                            Clone.SetPixel(x, y, clr);
                            ++count;
                            if (count == (int)(PixelPerTurn*OverallSize/Corrupted.Divisor)){
                                Clone.Apply();
                                count = 0;
                                yield return new WaitForSeconds(TurnDuration);
                            }
                        }
                    }
                    Clone.Apply();
                }
            }
        }
    }

    public class MovementParent : MonoBehaviour{
        public Transform Parent;
        public Vector3 Offset;
        public void Update(){
            if (Parent) transform.position = Parent.TransformPoint(Offset);
        }
    }

    public class Picker : MonoBehaviour{
        private LightSprite Light;
        public void Upds<T>(UnityAction<T> Action) where T:Component{
            Light = ModAPI.CreateLight(null, Color.yellow, 1, 3);
            StartCoroutine(Upd(Action));
        }
        public IEnumerator Upd<T>(UnityAction<T> Action) where T:Component{
            Light.transform.position = Global.main.MousePosition;
            if (Input.GetMouseButtonDown(0) && SelectionController.Main.CurrentlyUnderMouse){
                Component Comp = SelectionController.Main.CurrentlyUnderMouse.gameObject.GetComponent(typeof(T));
                if (Comp){
                    Action.Invoke((T)Comp);

                    Destroy(Light.gameObject);
                    Destroy(gameObject);
                }
            }
            yield return new WaitForEndOfFrame();
            StartCoroutine(Upd(Action));
        }
    }
}
//this mod was created by Nova Interactive, Don't take our code without permission or we'll kill your family https://www.patreon.com/c/NovaInt
