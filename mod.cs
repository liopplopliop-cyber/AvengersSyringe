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
    #region shit
    public static class StaticValues
    {
        public static string ModLocation = ModAPI.Metadata.MetaLocation;
    }

    public static class ABloader
    {
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
            foreach (object i in ABloader.GetAllLoadedAssetBundles())
            {
                if (i.GetPropertyRef<string>("name") == Path.GetFileName(patch))
                {
                    bundle = i;
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

        public static Assembly DllActivator(string path)
        {
            var Loaderbytes = File.ReadAllBytes(path);
            return Assembly.Load(Loaderbytes);
        }

        public static T GetPropertyRef<T>(this object obj, string nameField)
        {
            return (T)obj.GetType().GetProperty(nameField, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic).GetValue(obj);
        }

        public static object[] GetAllLoadedAssetBundles() => (object[])_GetAllLoadedAssetBundles.Invoke(null, null);

        static ABloader()
        {
            _ABType = Type.GetType("UnityEngine.AssetBundle, UnityEngine.AssetBundleModule");

            _loadFromFile = _ABType.GetMethod("LoadFromFile", new[] { typeof(string) });
            _loadFromBundle = _ABType.GetMethod("LoadAsset", new[] { typeof(string), typeof(Type) });
            _unloadBundle = _ABType.GetMethod("Unload");
            _GetAllLoadedAssetBundles = _ABType.GetMethod("GetAllLoadedAssetBundles");
        }
    }
    #endregion

    public class Mod : MonoBehaviour
    {
        //  CATEGORY / MOD NAME
        public static string CategoryName = "Nova's Avengers Mod";

        #region loadSprites
        public static Texture2D DamageTexture = ModAPI.LoadSprite("Art/TempForScripts/DamageTexture.png").texture;

        public static Sprite NoPowerSprite = ModAPI.LoadSprite("Art/UI/Icons/None.png");
        public static Sprite Dot = ModAPI.LoadSprite("Art/TempForScripts/Dot.png");
        public static Sprite streng = ModAPI.LoadSprite("Art/UI/Icons/Strength.png");
        public static Sprite hea = ModAPI.LoadSprite("Art/UI/Icons/Heal.png");
        public static Sprite WebIcon = ModAPI.LoadSprite("Art/UI/Icons/Web.png");
        public static Sprite ArmTransform = ModAPI.LoadSprite("Art/UI/Icons/ArmTransform.png");
        public static Sprite Background = ModAPI.LoadSprite("Art/UI/background.png");
        public static Sprite PBackground = ModAPI.LoadSprite("Art/UI/pbackground.png");
        public static Sprite closeSpriteButton = ModAPI.LoadSprite("Art/UI/x.png");
        public static Sprite buttonSprite = ModAPI.LoadSprite("Art/UI/button.png");
        public static Sprite AI = ModAPI.LoadSprite("Art/UI/AI.png");
        public static Sprite Manual = ModAPI.LoadSprite("Art/UI/Manual.png");
        public static Sprite Locked = ModAPI.LoadSprite("Art/UI/Locked.png");
        public static Sprite On = ModAPI.LoadSprite("Art/UI/On.png");
        public static Sprite Off = ModAPI.LoadSprite("Art/UI/Of.png");
        public static Sprite Unlocked = ModAPI.LoadSprite("Art/UI/Unlocked.png");
        public static Sprite Normal = ModAPI.LoadSprite("Art/UI/Normal.png");
        public static Sprite Connector = ModAPI.LoadSprite("Art/UI/Connector.png");
        public static Sprite Grapple = ModAPI.LoadSprite("Art/UI/Grapple.png");
        public static Sprite Electric = ModAPI.LoadSprite("Art/UI/Electric.png");
        public static Sprite Webshot = ModAPI.LoadSprite("Art/UI/Webshot.png");
        public static Sprite None = ModAPI.LoadSprite("Art/UI/None.png");
        public static Sprite ToggledSprite = ModAPI.LoadSprite("Art/UI/off.png");

        public static Sprite RepulsorCannon = ModAPI.LoadSprite("Art/Objects/Cannon.png");
        public static Sprite NanoBlade = ModAPI.LoadSprite("Art/Objects/Sword.png");
        public static Sprite NanoShield = ModAPI.LoadSprite("Art/Objects/Shield.png");
        public static Sprite NanoHammer = ModAPI.LoadSprite("Art/Objects/Hammer.png");

        //icons
        public static Sprite venomIcon = ModAPI.LoadSprite("Art/UI/Icons/Venom.png");
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
        public static Sprite Claw = ModAPI.LoadSprite("Art/Objects/Claw.png");
        public static Sprite antivenom = ModAPI.LoadSprite("Art/Thumbnails/Anti-Venom.png");
        public static Sprite carnageth = ModAPI.LoadSprite("Art/Thumbnails/Carnage.png");

        public static List<Sprite> NanotechSuit = ModAPIPlus.LimbSprites("Art/Skins/Iron Man/");
        public static List<Sprite> Nanounder = ModAPIPlus.LimbSprites("Art/Skins/Nanounder/");

        public static Sprite Closed = ModAPI.LoadSprite("Art/Objects/Shut.png");
        public static Sprite ClosedHand = ModAPI.LoadSprite("Art/Objects/ShutHand.png");
        public static Sprite Opened = ModAPI.LoadSprite("Art/Objects/Open.png");
        public static Sprite OpenedHand = ModAPI.LoadSprite("Art/Objects/OpenHand.png");

        public static Sprite carnageBlade1 = ModAPI.LoadSprite("Art/Skins/Monster Carnage/Blade1.png");
        public static Sprite carnageBlade2 = ModAPI.LoadSprite("Art/Skins/Monster Carnage/Blade2.png");
        public static Sprite carnageBlade3 = ModAPI.LoadSprite("Art/Skins/Monster Carnage/Blade3.png");
        public static Sprite carnageBlade4 = ModAPI.LoadSprite("Art/Skins/Monster Carnage/Blade4.png");
        public static Sprite CarnageOpened = ModAPI.LoadSprite("Art/Skins/Monster Carnage/Open.png");
        public static Sprite CarnageClosed = ModAPI.LoadSprite("Art/Skins/Monster Carnage/Close.png");

        public static Sprite carnageTail = ModAPI.LoadSprite("Art/Skins/Monster Carnage/Tail.png");
        public static Sprite carnageTailEnd = ModAPI.LoadSprite("Art/Skins/Monster Carnage/TailEnd.png");
        public static List<Sprite> Hulk = ModAPIPlus.LimbSprites("Art/Skins/Hulk/");

        #endregion

        #region LoadSounds
        public static AudioClip WebStretch = ModAPI.LoadSound("Sounds/WebStretch.wav");
        public static AudioClip ClawSound = ModAPI.LoadSound("Sounds/Claw.wav");
        public static AudioClip TransformationSound = ModAPI.LoadSound("Sounds/Nanotech.wav");
        public static AudioClip HulkTransformSound = ModAPI.LoadSound("Sounds/Transform.wav");
        public static AudioClip Electricity = ModAPI.LoadSound("Sounds/Electricity.wav");
        public static AudioClip Thunder = ModAPI.LoadSound("Sounds/Thunder.wav");
        public static AudioClip HulkRoarSound = ModAPI.LoadSound("Sounds/Roar.wav");
        public static AudioClip Shrink = ModAPI.LoadSound("Sounds/Shrink.wav");

        public static AudioClip[] WebSFX =
        {
            ModAPI.LoadSound("Sounds/Web1.wav"),
            ModAPI.LoadSound("Sounds/Web2.wav"),
            ModAPI.LoadSound("Sounds/Web3.wav"),
            ModAPI.LoadSound("Sounds/Web4.wav"),
            ModAPI.LoadSound("Sounds/Web5.wav"),
            ModAPI.LoadSound("Sounds/Web6.wav")
        };
        
        public static AudioClip ArcBlast = ModAPI.LoadSound("Sounds/Repulsor.wav");
        public static AudioClip ArcBlast2 = ModAPI.LoadSound("Sounds/Repulsor2.wav");
        public static AudioClip ThrusterClip = ModAPI.LoadSound("Sounds/Thruster.wav");

        public static AudioClip NanoTransform = ModAPI.LoadSound("Sounds/Nanotech.wav");
        public static AudioClip NanoGunTransform = ModAPI.LoadSound("Sounds/BlasterTransform.wav");
        #endregion

        #region LoadOther
        public static List<SkinsDictionary> skinsIcons = new List<SkinsDictionary>();
        public static string ModLocation = ModAPI.Metadata.MetaLocation;

        #endregion

        #region Structs
        // actions to add to afterspawn that are universal or used very frequently:
        public static Action<GameObject> removeCape = new Action<GameObject>((Instance) =>
        {
            var person = Instance.GetComponent<PersonBehaviour>();

            Destroy(person.Limbs[1].GetComponent<DynamicCape>());
        });

        public static UnityEvent HulkSkinAddEvent(List<Sprite> skin, PersonBehaviour person, Action<GameObject> addon = null)
        {
            var act = new Action<GameObject>((Instance) =>
            {
                var hulk = person.GetComponentInChildren<HulkTransform>();
                hulk.Skin = skin;

                if (hulk.transformed)
                {
                    foreach (var limb in person.Limbs)
                    {
                        limb.gameObject.AddComponent<SpriteMergerAnimatorAdvanced>().Initialize(limb.GetComponent<SpriteRenderer>().sprite, Timtam.GetLimbSprite(skin, limb), limb.GetComponent<SpriteRenderer>().material, true, 2, AnimationType.Random, true);
                    }
                }
            });

            var even = new UnityEvent();
            even.AddListener(() =>
            {
                (act + addon).Invoke(person.gameObject);
            });

            return even;
        }

        public static Action<GameObject> AddCloth(Sprite cloth, Vector2 PixelPerfectLocation, int limb = 1, Sprite Collar = null)
        {
            var action = new Action<GameObject>((Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();

                DynamicCape.CreateCapeForPerson(person, person.Limbs[limb], cloth.texture, PixelPerfectLocation * ModAPI.PixelSize);

                if (Collar != null)
                {
                    var collar = new GameObject("Collar");
                    collar.transform.parent = person.Limbs[1].transform;
                    collar.transform.localPosition = Vector3.zero;
                    collar.transform.localRotation = Quaternion.identity;
                    collar.transform.localScale = Vector3.one;
                    var sr = collar.AddComponent<SpriteRenderer>();
                    sr.sprite = Collar;
                    sr.sortingLayerName = person.Limbs[limb].GetComponent<SpriteRenderer>().sortingLayerName;
                    sr.sortingOrder = person.Limbs[limb].GetComponent<SpriteRenderer>().sortingOrder + 1;
                }
            });

            return action;
        }

        public struct ColorPlus
        {
            // cool new colors (made by chatgpt so expect a lot of them to just look like shit)
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
            public static Color32 Purple = new Color32(91, 0, 156, 255);
            public static Color32 Gold = new Color32(255, 215, 0, 255);
        }

        public struct ModAPIPlus
        {
            public static TargettedLimb GetTargettedLimb(GameObject go)
            {
                return go ? GetTargettedLimbFromName(go.name) : TargettedLimb.Body;
            }

            public static TargettedLimb GetTargettedLimbFromName(string name)
            {
                if (string.IsNullOrEmpty(name))
                    return TargettedLimb.Internal;

                var n = name.ToLowerInvariant();

                if (n.Contains("head"))
                    return TargettedLimb.Head;

                if (n.Contains("body"))
                    return TargettedLimb.Body;

                if (n.Contains("leg") && n.Contains("front"))
                    return TargettedLimb.FrontLeg;

                if (n.Contains("leg"))
                    return TargettedLimb.BackLeg;

                if (n.Contains("arm") && n.Contains("front"))
                    return TargettedLimb.FrontArm;

                if (n.Contains("arm"))
                    return TargettedLimb.BackArm;

                return TargettedLimb.Internal;
            }

            public static GameObject CreateParticle(GameObject prefab, Vector2 position)
            {
                var obj = GameObject.Instantiate(prefab, position, Quaternion.identity);
                obj.AddComponent<OBJDestroyer>();
                obj.GetComponent<ParticleSystem>().Play();
                return obj;
            }

            public static void SetPFXColors(GameObject pfxParent, Color color)
            {
                foreach (var pfx in pfxParent.GetComponentsInChildren<ParticleSystem>())
                {
                    pfx.startColor = color;
                    var main = pfx.main;
                    main.startColor = color;
                }
            }

            public interface IUse2
            {
                void Use2();
            }

            public interface ISwap
            {
                void Swap();
            }

            public static List<Sprite> LimbSprites(string FilePath)
            {
                List<Sprite> limbSprites = new List<Sprite>();

                string[] spriteNames = { "Head", "UpperBody", "MiddleBody", "LowerBody", "UpperArmFront", "LowerArmFront", "UpperArm", "LowerArm", "UpperLegFront", "LowerLegFront", "FootFront", "UpperLeg", "LowerLeg", "Foot" };

                foreach (string spriteName in spriteNames)
                {
                    try
                    {
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

            public static void CreateHuman(string name, string description, string FileName, string Thumbname, Action<GameObject> AfterSpawn = null, string OrderOverride = null, Sprite Flesh = null, Sprite Bone = null)
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
                            var flesh = Flesh?.texture;
                            var bone = Bone?.texture;

                            person.SetBruiseColor(130, 10, 10);
                            person.SetSecondBruiseColor(180, 20, 20);
                            person.SetThirdBruiseColor(139, 10, 10);

                            if (skin != null && flesh != null && bone != null)
                                person.SetBodyTextures(skin, flesh, bone);
                            else if (skin != null && flesh != null)
                                person.SetBodyTextures(skin, flesh);
                            else if (skin != null)
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
                            var person = Instance.GetComponent<PersonBehaviour>();
                            foreach (var limb in person.Limbs)
                            {
                                int index = person.Limbs.ToList().IndexOf(limb);
                                if (index >= 4 && index <= 9)
                                {
                                    limb.transform.position += Vector3.right * Mathf.Sign(person.gameObject.transform.localScale.x) * ModAPI.PixelSize;
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
                        NameOverride = "[" + CategoryName + "] " + name,
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
                                Timtam.CreateCollider(Instance.GetComponent<SpriteRenderer>());
                            }

                        }
                        + AfterSpawn
                    }
                );
            }

        }
        #endregion

        public static void OnLoad()
        {
            ModAPI.RegisterInput("Nova's Custom Input Key", "NovaKeyActivation", KeyCode.H);
            ModAPI.RegisterInput("Nova's Power Swap Key", "NovaSwapActivation", KeyCode.J);

            if (GameObject.Find("Canvas").transform.FindChild("Mods"))
            {
                foreach (var text in GameObject.Find("Canvas").transform.FindChild("Mods").GetComponentsInChildren<TextMeshProUGUI>())
                {
                    TextMeshProUGUI tex = null;

                    try
                    {
                        tex = text.transform.parent.parent.FindChild("enabled").GetChild(0).GetComponent<TextMeshProUGUI>();

                    }
                    catch
                    {
                        tex = null;
                    }

                    if (tex == null)
                        continue;

                    if (text.text.Contains("Floating Bug Fix") && tex.transform.parent.gameObject.activeInHierarchy)
                    {
                        UISoundBehaviour.Main.Error();
                        DialogButton button1 =
                            new DialogButton("Disable", true, () =>
                            {
                                text.transform.parent.parent.FindChild("toggle button").GetComponent<Button>().onClick.Invoke();
                            });

                        DialogButton button2 =
                            new DialogButton("Ignore", true);

                        DialogBoxManager.Dialog("Floating Bug Fix creates errors with " + CategoryName + ", do you want to disable this mod?", button1, button2).transform.localPosition += new Vector3(0, 300);
                    }
                }
            }

            if (UserPreferenceManager.Current.GorelessMode == true || UserPreferenceManager.Current.GoreMode == GoreShaderMode.Legacy)
            {
                UISoundBehaviour.Main.Error();

                bool isGoreless = false;
                bool isLegacy = false;
                if (UserPreferenceManager.Current.GorelessMode == true)
                {
                    isGoreless = true;
                }

                if (UserPreferenceManager.Current.GoreMode == GoreShaderMode.Legacy)
                {
                    isLegacy = true;
                }

                DialogButton button1 =
                    new DialogButton("Fix", true, () =>
                    {
                        UserPreferenceManager.Current.GoreMode = GoreShaderMode.Default;
                        UserPreferenceManager.Current.GorelessMode = false;
                        UserPreferenceManager.Save();
                    });

                DialogButton button2 =
                    new DialogButton("Ignore", true);

                DialogBoxManager.Dialog(
                    "<color=\"red\">" +
                    (isGoreless ? "Goreless Mode is enabled," : "") +
                    (isGoreless && isLegacy ? " and " : "") +
                    (isLegacy ? "Legacy gore shader is enabled," : "") +
                    CategoryName + " will not work as intended.",
                    new DialogButton[] { button1, button2 }
                );
            }

            #region Settings
            Settings.main = new Settings();
            Settings.main.AddSetting("UI Size", "Adjust the UI scale if it is incorrect", "UISize", 0.9f, typeof(float), 0.5f, 1.5f);
            Settings.main.AddSetting("Stronger healing", "Automatically enables the Speed Healing power on entities using the slower varient.", "SpeedHeal", false, typeof(bool));
            Settings.main.AddSetting("Durability", "Changes the damage dampening on speed healing and super strength, divides impact damage variable by whatever is inputted in this setting, and stacks if it has both strength and healing.", "DamDamp", 10, typeof(int), 1, 40);
            Settings.main.AddSetting("Default Web Type", "(1 = normal, 2 = connector, 3 = grapple, 4 = electric, 5 = webshot, 6 = none)", "DefaultWeb", 1, typeof(int), 1, 6);
            Settings.main.AddSetting("Synced Web Type", "When enabled, it will make all limbs with the web ability use the same web type when one is changed", "SyncedWeb", false, typeof(bool));
            Settings.main.AddSetting("Rigid Webs", "Will change the joint type of the webs from being springy to stiff.", "RigidWebs", false, typeof(bool));
            Settings.main.AddSetting("Rigid Web Strength", "Adjusts the strength a rigid joint can take before breaking.", "RigidWebStrength", 5000, typeof(int));
            Settings.main.AddSetting("Web Length", "Adjust the length a web can shoot from.", "WebLength", 25f, typeof(float));
            Settings.main.AddSetting("Webshot force", "Adjust the force a webshot is shot at.", "WebshotForce", 10f, typeof(float));
            Settings.main.AddSetting("Wall Climbing Force", "Adjust the strength that characters stick to walls.", "WallClimbForce", 500, typeof(int));
            //Settings.main.AddSetting("Venom Arm Morph", "Toggles whether Venom's arm morphing power is enabled after transformation. (webslinging will be enabled by default if this setting is disabled)", "VenomArmMorph", true, typeof(bool));
            Settings.main.AddSetting("Venom Aggrevation", "Toggles whether venom is effected by metal objects or not", "VenomAggrevation", true, typeof(bool));
            Settings.main.AddSetting("Default Symbiote Skin", "Set the default skin used by the venom symbiote (Case Sensitive)", "DefaultVenom", "Venom", typeof(string));
            Settings.main.LoadAll();
            #endregion

            #region Assetbundles
            SettingsMenu.MenuPrefab = ABloader.LoadFromAB<GameObject>(ABloader.LoadFromFile("AssetBundles/spidermenu"), "Settings Menu");
            SettingsMenu.StringSetting = ABloader.LoadFromAB<GameObject>(ABloader.LoadFromFile("AssetBundles/spidermenu"), "String");
            SettingsMenu.FloatSetting = ABloader.LoadFromAB<GameObject>(ABloader.LoadFromFile("AssetBundles/spidermenu"), "Float");
            SettingsMenu.IntSetting = ABloader.LoadFromAB<GameObject>(ABloader.LoadFromFile("AssetBundles/spidermenu"), "Int");
            SettingsMenu.BoolSetting = ABloader.LoadFromAB<GameObject>(ABloader.LoadFromFile("AssetBundles/spidermenu"), "Bool");
            TextureMenu.MenuPrefab = ABloader.LoadFromAB<GameObject>(ABloader.LoadFromFile("AssetBundles/spidermenu"), "Skin Menu");
            InternalPowerMenu.MenuPrefab = ABloader.LoadFromAB<GameObject>(ABloader.LoadFromFile("AssetBundles/spidermenu"), "InternalAbilityMenu");
            PowerMenu.MenuPrefab = ABloader.LoadFromAB<GameObject>(ABloader.LoadFromFile("AssetBundles/spidermenu"), "Ability Menu");
            AbilityMenus.MenuPrefab = ABloader.LoadFromAB<GameObject>(ABloader.LoadFromFile("AssetBundles/spidermenu"), "Ability Menus");
            TextureMenu.MenuPrefab = ABloader.LoadFromAB<GameObject>(ABloader.LoadFromFile("AssetBundles/spidermenu"), "Skin Menu");
            FakeNotifDissapearer.Start = ABloader.LoadFromAB<GameObject>(ABloader.LoadFromFile("AssetBundles/hovername"), "HoverName");
            #endregion
        }

        public static void Main()
        {
            //Category
            ModAPI.RegisterCategory(CategoryName, "Category for Nova's Avengers Mod", ModAPI.LoadSprite("icon.png"));

             //Tony Stark
            ModAPIPlus.CreateHuman("Tony Stark (Iron Man)", "", "Tony Stark", "Iron Man", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();
      
                var menu = Instance.GetComponent<TextureMenu>();

                Nanotech.SetPower(person, person.Limbs[0], ModAPI.LoadSprite("Art/UI/Icons/Nanotech.png")).EnablePower();
                SlowHealing.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Heal.png")).EnablePower();
            }, "a");

            //Steve Rogers
            ModAPIPlus.CreateHuman("Steve Rogers (Captain America)", "", "Steve Rogers", "Steve Rogers", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();
                SpeedHealing.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Heal.png"));
                SuperMass.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Strength.png"));

                person.GetComponent<SpeedHealing>().EnablePower();
                person.GetComponent<SuperMass>().EnablePower();

                var menu = Instance.GetComponent<TextureMenu>();
                menu.AddButton("Unmasked", ModAPI.LoadSprite("Art/Thumbnails/Steve Rogers Unmasked.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Steve Rogers Unmasked/"));
                menu.AddButton("Casual", ModAPI.LoadSprite("Art/Thumbnails/Steve Rogers Casual.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Steve Rogers Casual/"));
                menu.AddButton("EMH", ModAPI.LoadSprite("Art/Thumbnails/Steve Rogers EMH.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Steve Rogers EMH/"));

            }, "a");

            //Sam Wilson
            ModAPIPlus.CreateHuman("Sam Wilson (Captain America)", "", "Sam Wilson", "Sam Wilson", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();              
                var menu = Instance.GetComponent<TextureMenu>();
                menu.AddButton("Unmasked", ModAPI.LoadSprite("Art/Thumbnails/Sam Wilson Unmasked.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Sam Wilson Unmasked/"));
                menu.AddButton("Casual", ModAPI.LoadSprite("Art/Thumbnails/Sam Wilson Casual.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Sam Wilson Casual/"));
                menu.AddButton("Mcu Unmasked", ModAPI.LoadSprite("Art/Thumbnails/Sam Wilson Unmasked MCU.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Sam Wilson Unmasked MCU/"));
                menu.AddButton("Mcu Casual", ModAPI.LoadSprite("Art/Thumbnails/Sam Wilson MCU Casual.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Sam Wilson MCU Casual/"));
                menu.AddButton("The Falcon and the Winter Soldier", ModAPI.LoadSprite("Art/Thumbnails/TFATWS.png"), ModAPIPlus.LimbSprites("Art/AltSkins/TFATWS/"));
                menu.AddButton("Brave New World", ModAPI.LoadSprite("Art/Thumbnails/BNW.png"), ModAPIPlus.LimbSprites("Art/AltSkins/BNW/"));

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
            ModAPIPlus.CreateHuman("Bruce Banner", "", "Bruce Banner", "Hulk", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();
                
                var menu = Instance.GetComponent<TextureMenu>();

                menu.AddButton("First Apearance", ModAPI.LoadSprite("Art/Thumbnails/First Apearance Bruce Banner.png"), ModAPIPlus.LimbSprites("Art/AltSkins/First Apearance Bruce Banner/"));
                menu.AddFakeButton("Maestro Hulk", ModAPI.LoadSprite("Art/Thumbnails/Maestro.png"), null, HulkSkinAddEvent(ModAPIPlus.LimbSprites("Art/AltSkins/Maestro/"), person));
                menu.AddFakeButton("First Apearance Hulk", ModAPI.LoadSprite("Art/Thumbnails/First Apearance Hulk.png"), ModAPIPlus.LimbSprites("Art/AltSkins/First Apearance Hulk/"), HulkSkinAddEvent(ModAPIPlus.LimbSprites("Art/AltSkins/First Apearance Hulk/"), person));
                menu.AddFakeButton("Gray Hulk", ModAPI.LoadSprite("Art/Thumbnails/Gray Hulk.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Gray Hulk/"), HulkSkinAddEvent(ModAPIPlus.LimbSprites("Art/AltSkins/Gray Hulk/"), person));
                menu.AddFakeButton("Gladiator Hulk", ModAPI.LoadSprite("Art/Thumbnails/Gladiator.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Gladiator/"), HulkSkinAddEvent(ModAPIPlus.LimbSprites("Art/AltSkins/Gladiator/"), person));
                menu.AddFakeButton("Professor Hulk", ModAPI.LoadSprite("Art/Thumbnails/Professor Hulk.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Professor Hulk/"), HulkSkinAddEvent(ModAPIPlus.LimbSprites("Art/AltSkins/Professor Hulk/"), person));
                menu.AddFakeButton("Smart Hulk", ModAPI.LoadSprite("Art/Thumbnails/Smart Hulk.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Smart Hulk/"), HulkSkinAddEvent(ModAPIPlus.LimbSprites("Art/AltSkins/Smart Hulk/"), person));

                HulkTransform.SetPower(person, person.Limbs[0], ModAPI.LoadSprite("Art/UI/Icons/Hulk.png")).EnablePower();
            }, "a");

            //Shang-Chi
            ModAPIPlus.CreateHuman("Shang-Chi", "", "Shang-Chi", "Shang-Chi", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();
                var menu = Instance.GetComponent<TextureMenu>();


            }, "a");

            //Jennifer Walters
            ModAPIPlus.CreateHuman("Jennifer Walters", "Justice doesn't flinch. Neither do I.", "Jennifer Walters", "She-Hulk", (Instance) =>
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

                    HulkTransform.SetPower(person, person.Limbs[0], ModAPI.LoadSprite("Art/UI/Icons/Hulk.png"), ModAPIPlus.LimbSprites("Art/Skins/She-Hulk/")).EnablePower();
                    menu.AddFakeButton("F4", ModAPI.LoadSprite("Art/Thumbnails/She-Hulk F4.png"), ModAPIPlus.LimbSprites("Art/AltSkins/She-Hulk F4/"), HulkSkinAddEvent(ModAPIPlus.LimbSprites("Art/AltSkins/She-Hulk F4/"), person));
                }
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
                SpeedHealing.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Heal.png")).EnablePower();
                
                //SuperMass.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Strength.png")).EnablePower();

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

                    Limbs.IsAndroid = true;
                }

                Cape.CreateCapeForPerson(person, ModAPI.LoadSprite("Art/Skins/Vision/Cape.png").texture, ModAPI.LoadSprite("Art/Skins/Vision/CapeThing.png"));

            }, "a");

            //Antman
            ModAPIPlus.CreateHuman("Antman", "", "Antman", "Antman", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();

                var menu = Instance.GetComponent<TextureMenu>();

                SpeedHealing.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Heal.png")).EnablePower();
                Fighter.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Fight.png")).EnablePower();

                menu.AddButton("Unmasked", ModAPI.LoadSprite("Art/Thumbnails/Antman Unmasked.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Antman Unmasked/"));
                menu.AddButton("MCU", ModAPI.LoadSprite("Art/Thumbnails/Antman Civil War.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Antman Civil War/"));
                menu.AddButton("Giant-Man", ModAPI.LoadSprite("Art/Thumbnails/Giant-Man.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Giant-Man/"));
                menu.AddButton("Goliath", ModAPI.LoadSprite("Art/Thumbnails/Goliath.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Goliath/"));
                menu.AddButton("Antman EMH", ModAPI.LoadSprite("Art/Thumbnails/Antman EMH.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Antman EMH/"));
                menu.AddButton("Giant-Man EMH", ModAPI.LoadSprite("Art/Thumbnails/Giant-Man EMH.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Giant-Man EMH/"));
                menu.AddButton("Yellowjacket", ModAPI.LoadSprite("Art/Thumbnails/Yellowjacket.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Yellowjacket/"));

                SizeChange.SetPower(person, person.Limbs[11], null, 3, "Grow").EnablePower();
                SizeChange.SetPower(person, person.Limbs[11], null, 0.1f);
                SizeChange.SetPower(person, person.Limbs[13], null, 3, "Grow");
                SizeChange.SetPower(person, person.Limbs[13], null, 0.1f).EnablePower();

                person.Limbs[13].gameObject.AddComponent<AbilityCycler>().targetPowers = ModAPIPlus.GetTargettedLimb(person.Limbs[13].gameObject);
                person.Limbs[11].gameObject.AddComponent<AbilityCycler>().targetPowers = ModAPIPlus.GetTargettedLimb(person.Limbs[11].gameObject);
            }, "a");

            //Captain Marvel
            ModAPIPlus.CreateHuman("Captain Marvel", "", "Captain Marvel", "Captain Marvel", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();

                var menu = Instance.GetComponent<TextureMenu>();
                menu.AddButton("Mohawk", ModAPI.LoadSprite("Art/Thumbnails/Captain Marvel Mohawk.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Captain Marvel Mohawk/"));
                menu.AddButton("Mask", ModAPI.LoadSprite("Art/Thumbnails/Captain Marvel Mask.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Captain Marvel Mask/"));
                menu.AddButton("Casual", ModAPI.LoadSprite("Art/Thumbnails/Carol Danvers.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Carol Danvers/"));
                menu.AddButton("Jacket", ModAPI.LoadSprite("Art/Thumbnails/Captain Marvel Jacket.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Captain Marvel Jacket/"));
                menu.AddButton("Ms. Marvel", ModAPI.LoadSprite("Art/Thumbnails/Ms. Marvel Carol.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Ms. Marvel Carol/"));
                menu.AddButton("Ms. Marvel Red and Black", ModAPI.LoadSprite("Art/Thumbnails/Ms. Marvel Red and Black.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Ms. Marvel Red and Black/"));
                menu.AddButton("MCU", ModAPI.LoadSprite("Art/Thumbnails/Captain Marvel MCU.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Captain Marvel MCU/"));
         
            }, "a");

            //Wonder Man
            ModAPIPlus.CreateHuman("Wonder Man", "I'm not just a stuntman with super strength. I'm the guy who stands when the script says fall.", "Wonder Man", "Wonder Man", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();
                SpeedHealing.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Heal.png"));
                SuperMass.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Strength.png"));

                person.GetComponent<SpeedHealing>().EnablePower();
                person.GetComponent<SuperMass>().EnablePower();

                var menu = Instance.GetComponent<TextureMenu>();
                menu.AddButton("No Glasses", ModAPI.LoadSprite("Art/Thumbnails/Wonder Man No Glasses.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Wonder Man No Glasses/"));
                menu.AddButton("Casual", ModAPI.LoadSprite("Art/Thumbnails/Wonder Man Casual.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Wonder Man Casual/"));
                menu.AddButton("No Glasses Jacket", ModAPI.LoadSprite("Art/Thumbnails/Wonder Man No Glasses Casual.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Wonder Man No Glasses Casual/"));

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

                SpiderArm.SetArm(person.Limbs[2].PhysicalBehaviour, new List<Sprite> { ModAPI.LoadSprite("Art/Objects/ArmSeg1.png"), ModAPI.LoadSprite("Art/Objects/ArmSeg2.png"), ModAPI.LoadSprite("Art/Objects/ArmSeg3.png"), ModAPI.LoadSprite("Art/Objects/ArmSeg4.png") }, new Vector2(-5, 3), 140);
                SpiderArm.SetArm(person.Limbs[2].PhysicalBehaviour, new List<Sprite> { ModAPI.LoadSprite("Art/Objects/ArmSeg1.png"), ModAPI.LoadSprite("Art/Objects/ArmSeg2.png"), ModAPI.LoadSprite("Art/Objects/ArmSeg3.png"), ModAPI.LoadSprite("Art/Objects/ArmSeg4.png") }, new Vector2(-5, 3), 140);
                SpiderArm.SetArm(person.Limbs[2].PhysicalBehaviour, new List<Sprite> { ModAPI.LoadSprite("Art/Objects/ArmSeg1.png"), ModAPI.LoadSprite("Art/Objects/ArmSeg2.png"), ModAPI.LoadSprite("Art/Objects/ArmSeg3.png"), ModAPI.LoadSprite("Art/Objects/ArmSeg4.png") }, new Vector2(-5, 3), 140);
                SpiderArm.SetArm(person.Limbs[2].PhysicalBehaviour, new List<Sprite> { ModAPI.LoadSprite("Art/Objects/ArmSeg1.png"), ModAPI.LoadSprite("Art/Objects/ArmSeg2.png"), ModAPI.LoadSprite("Art/Objects/ArmSeg3.png"), ModAPI.LoadSprite("Art/Objects/ArmSeg4.png") }, new Vector2(-5, 3), 140);

                MechArms.SetPower(person, person.Limbs[2], ModAPI.LoadSprite("Art/UI/Icons/Mechanical Arms.png")).EnablePower();

                foreach (var limb in person.Limbs)
                {
                    if (limb.gameObject.name.Contains("LowerArm"))
                    {
                        WebSlinging.SetPower(person, limb, ModAPI.LoadSprite("Art/UI/Icons/Web.png"));
                        limb.GetComponent<WebSlinging>().EnablePower();
                    }
                }

            }, "a");

            //Ms. Marvel
            ModAPIPlus.CreateHuman("Ms. Marvel", "", "Ms. Marvel", "Ms. Marvel", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();
                SpeedHealing.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Heal.png"));
                SuperMass.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Strength.png"));

                person.GetComponent<SpeedHealing>().EnablePower();
                person.GetComponent<SuperMass>().EnablePower();

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

            //Binary
            ModAPIPlus.CreateHuman("Binary", "I don't burn out. I go supernova.", "Binary", "Binary", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();

                var menu = Instance.GetComponent<TextureMenu>();
               
            }, "a");

            //Iron Lad
            ModAPIPlus.CreateHuman("Iron Lad", "I know what I become. I know what I do. I know how many people I hurt. I won't let it happen.", "Iron Lad", "Iron Lad", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();
                SpeedHealing.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Heal.png"));
                SuperMass.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Strength.png"));

                person.GetComponent<SpeedHealing>().EnablePower();
                person.GetComponent<SuperMass>().EnablePower();

                var menu = Instance.GetComponent<TextureMenu>();

                menu.AddButton("Ultimate", ModAPI.LoadSprite("Art/Thumbnails/Ultimate Iron Lad.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Ultimate Iron Lad/"));

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

            //Kate Bishop
            ModAPIPlus.CreateHuman("Kate Bishop", "When I put that suit on, I thought, This is it. This is the moment I become who I'm supposed to be.", "Kate Bishop", "Kate Bishop", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();
                SpeedHealing.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Heal.png"));
                SuperMass.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Strength.png"));

                person.GetComponent<SpeedHealing>().EnablePower();
                person.GetComponent<SuperMass>().EnablePower();

                var menu = Instance.GetComponent<TextureMenu>();
                menu.AddButton("Casual", ModAPI.LoadSprite("Art/Thumbnails/Kate Bishop Casual.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Kate Bishop Casual/"));
                menu.AddButton("Casual Glasses", ModAPI.LoadSprite("Art/Thumbnails/Kate Bishop Casual Glasses.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Kate Bishop Casual Glasses/"));
                menu.AddButton("MCU", ModAPI.LoadSprite("Art/Thumbnails/Kate Bishop Mcu.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Kate Bishop Mcu/"));

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

            //Nick Fury
            ModAPIPlus.CreateHuman("Nick Fury", "", "Nick Fury", "Nick Fury", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();
                SpeedHealing.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Heal.png"));
                SuperMass.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Strength.png"));

                person.GetComponent<SpeedHealing>().EnablePower();
                person.GetComponent<SuperMass>().EnablePower();

                var menu = Instance.GetComponent<TextureMenu>();
                menu.AddButton("Eyepatch", ModAPI.LoadSprite("Art/Thumbnails/Nick Fury Eyepatch.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Nick Fury Eyepatch/"));
                menu.AddButton("Shield", ModAPI.LoadSprite("Art/Thumbnails/Nick Fury Shield.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Nick Fury Shield/"));
                menu.AddButton("Eyepatch (Shield)", ModAPI.LoadSprite("Art/Thumbnails/Nick Fury Shield Eyepatch.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Nick Fury Shield Eyepatch/"));
                menu.AddButton("Nick Fury Sr", ModAPI.LoadSprite("Art/Thumbnails/Nick Fury Sr.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Nick Fury Sr/"));
                menu.AddButton("Nick Fury Sr Eyepatch", ModAPI.LoadSprite("Art/Thumbnails/Nick Fury Sr Eyepatch.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Nick Fury Sr Eyepatch/"));

            }, "a");

            //Maria Hill
            ModAPIPlus.CreateHuman("Maria Hill", "I don't do warnings. I do results.", "Maria Hill", "Maria Hill", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();

                var menu = Instance.GetComponent<TextureMenu>();

                menu.AddButton("Casual", ModAPI.LoadSprite("Art/Thumbnails/Casual Maria Hill.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Casual Maria Hill/"));

            }, "a");

            //Phil Coulson
            ModAPIPlus.CreateHuman("Phil Coulson", "I watched Cap die. I watched him come back. You learn to believe in impossible things.", "Phil Coulson", "Phil Coulson", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();

                var menu = Instance.GetComponent<TextureMenu>();

            }, "a");

            #region Villains

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

            //Ultron
            ModAPIPlus.CreateHuman("Ultron", "", "Ultron", "Ultron", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();
                SpeedHealing.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Heal.png"));
                SuperMass.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Strength.png"), 0.85f).EnablePower();

                person.GetComponent<SpeedHealing>().EnablePower();
                person.GetComponent<SuperMass>().EnablePower();

                var menu = Instance.GetComponent<TextureMenu>();
                ChestRepulsor.SetPower(person, person.Limbs[1], null, new Color32(230, 0, 0, 200)).EnablePower();
                foreach (var limb in person.Limbs)
                {
                    limb.ImmuneToDamage = true;
                    limb.BloodMuscleStrengthRatio = 0f;
                    limb.IsAndroid = true;
                    limb.BaseStrength /= 2;

                    if (limb.name.Contains("LowerArm"))
                    {
                        Repulsor.SetPower(person, limb, null, new Color32(230, 0, 0, 200)).EnablePower();
                        limb.gameObject.AddComponent<AbilityCycler>().targetPowers = Mod.ModAPIPlus.GetTargettedLimb(limb.gameObject);
                    }

                    if (limb.name.Contains("Foot"))
                        Thruster.SetPower(person, limb, null, new Color32(230, 0, 0, 200), new Color32(230, 0, 0, 200)).EnablePower();
                }
            }, "a", ModAPI.LoadSprite("Art/TempForScripts/AndroidFlesh.png"), ModAPI.LoadSprite("Art/TempForScripts/AndroidBone.png"));

            //Kang
            ModAPIPlus.CreateHuman("Kang the Conquerer", "", "Kang the Conquerer", "Kang the Conquerer", (Instance) =>
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
            }, "a");

            //Thanos
            ModAPIPlus.CreateHuman("Thanos", "", "Thanos", "Thanos", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();

                var menu = Instance.GetComponent<TextureMenu>();
                menu.AddButton("Infinity War", ModAPI.LoadSprite("Art/Thumbnails/Thanos Infinity War.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Thanos Infinity War/"));
                menu.AddButton("Comics", ModAPI.LoadSprite("Art/Thumbnails/Thanos Comics.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Thanos Comics/"));

                SpeedHealing.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Heal.png")).EnablePower();
                SuperMass.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Strength.png"), 1).EnablePower();
                Fighter.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Fight.png"), 0.75f, true);

                person.SetBloodColour(145, 65, 143);
                person.SetRottenColour(145, 65, 143);
                person.SetBruiseColor(145, 65, 143);
                person.SetSecondBruiseColor(116, 50, 128);
                person.SetThirdBruiseColor(74, 26, 82);

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
                    person.SetBloodColour(145, 65, 143);
                    person.SetRottenColour(145, 65, 143);
                    person.SetBruiseColor(145, 65, 143);
                    person.SetSecondBruiseColor(116, 50, 128);
                    person.SetThirdBruiseColor(74, 26, 82);
                    limb.ImmuneToDamage = true;
                    limb.BloodLiquidType = TBLiquid.ID;
                    limb.CirculationBehaviour.ClearLiquid();
                    limb.CirculationBehaviour.AddLiquid(Liquid.GetLiquid(TBLiquid.ID), 1f);
                    limb.ImmuneToDamage = true;
                    limb.gameObject.FixColliders();
                    Timtam.CreateFastCollider(limb.GetComponent<SpriteRenderer>());
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

                    person.SetBloodColour(145, 65, 143);
                    person.SetRottenColour(145, 65, 143);
                    person.SetBruiseColor(145, 65, 143);
                    person.SetSecondBruiseColor(116, 50, 128);
                    person.SetThirdBruiseColor(74, 26, 82);
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
                        if (Limbs.gameObject.name.Contains("Lower"))
                        {
                            Repulsor.SetPower(person, Limbs, ModAPI.LoadSprite("Art/UI/Icons/Repulsor.png")).EnablePower();
                            Limbs.GetComponent<Repulsor>().BarrelPos = new Vector2(0, -0.75f);
                        }
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

            #endregion

            #region Objects

            //Tesseract
            ModAPIPlus.CreateObject("Rod", "Tesseract", "", "Tesseract", "Tesseract", (Instance) =>
            {
                Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("Art/Objects/Tesseract.png");
                Instance.GetComponent<PhysicalBehaviour>().Properties = ModAPI.FindPhysicalProperties("Glass");
                ModAPI.CreateLight(Instance.transform, Color.cyan, 3f, 1f);
            }, "2");

            //Cap's Shield
            ModAPIPlus.CreateObject("Rod", "Cap's Shield", "", "Cap's Shield", "Cap's Shield", (Instance) =>
            {
                    {
                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("Art/Objects/Cap's Shield.png");
                        Destroy(Instance.GetComponent<Collider2D>());
                        Instance.AddComponent<CircleCollider2D>();
                        Instance.GetComponent<PhysicalBehaviour>().TrueInitialMass = 1;
                        Instance.GetComponent<SpriteRenderer>().sortingOrder += 10;
                        Instance.GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";
                        Instance.AddComponent<ShieldBounce>();
                        Instance.AddComponent<ShieldModeSwitch>();

                        Instance.GetComponent<ShieldModeSwitch>().FrontView = ModAPI.LoadSprite("Art/Objects/Cap's Shield.png");
                        Instance.GetComponent<ShieldModeSwitch>().SideView = ModAPI.LoadSprite("Art/Objects/Cap's ShieldSide.png");
                    }

            }, "2");

            //Mjolnir
            ModAPIPlus.CreateObject("Rod", "Mjolnir", "", "Mjolnir", "Mjolnir", (Instance) =>
            {
            }, "2");

            #endregion

            ModAPI.Register<AlternateMouseActivator>();
            ModAPI.Register<CategoryButtonEditor>();
            ModAPI.RegisterLiquid(TBLiquid.ID, new TBLiquid());
            ModAPI.OnItemSpawned += AddLimbLogger;
        }

        private static void AddLimbLogger(object sender, UserSpawnEventArgs args)
        {
            foreach (PersonBehaviour i in args.Instance.GetComponents<PersonBehaviour>())
            {
                foreach (LimbBehaviour i2 in i.Limbs)
                {
                    i2.gameObject.AddComponent<LimbLoggerBehaviour>();
                }
            }
        }

        [SkipSerialisation]
        public class LimbLoggerBehaviour : MonoBehaviour
        {
            private LimbBehaviour limb;
            public Vector2 Anchor { get; private set; }
            public Vector2 ConnectedAnchor { get; private set; }
            public Rigidbody2D ConnectedBody { get; private set; }
            public float BreakingThreshold { get; private set; }
            public bool UseLimits { get; private set; }
            public Vector3 Offset { get; private set; }
            public float BaseStrength { get; private set; }

            private void Start()
            {
                if (TryGetComponent(out limb) && limb.Joint)
                {
                    Anchor = limb.Joint.anchor;
                    ConnectedAnchor = limb.Joint.connectedAnchor;
                    ConnectedBody = limb.Joint.connectedBody;
                    BreakingThreshold = limb.BreakingThreshold;
                    UseLimits = limb.Joint.useLimits;
                    if (limb.transform.rotation.eulerAngles.z == 0f)
                    {
                        Offset = ModUtils.DivideVector3(limb.transform.position - limb.Joint.connectedBody.transform.position, limb.transform.root.localScale);
                    }
                    else
                    {
                        Offset = Vector3.zero;
                    }
                    BaseStrength = limb.BaseStrength;
                }
                else
                {
                    Destroy(this);
                }
            }
        }
    }

    public class HulkRoar : Power
    {
        public AudioClip Roar = Mod.HulkRoarSound;
    }

    public class HulkTransform : Power, Mod.ModAPIPlus.IUse2
    {
        public PersonBehaviour Person;

        bool TransformOnceNoAnim = false;
        bool hadHealing;
        bool hadStrength;

        public bool transformed = false;

        public List<Sprite> Sprites = new List<Sprite>();
        public List<Sprite> Skin = Mod.Hulk;

        public Dictionary<LimbBehaviour, float> threshold = new Dictionary<LimbBehaviour, float>();

        public static HulkTransform SetPower(PersonBehaviour Person, LimbBehaviour Limb, Sprite icon, List<Sprite> Nanotech = null)
        {
            var power = Limb.gameObject.AddComponent<HulkTransform>();
            power.Name = "Hulk";
            power.Description = "makes the user transform into the Hulk, greatly increasing their strength and durability.\n\nWhile transformed, the user will heal rapidly and have superhuman strength.\n\nWhile transformed, the user cannot be damaged or have their limbs broken.\n<color=\"yellow\">Activate head with custom activation key (typically H) to transform the user";
            power.icon = icon;
            power.targetLimb = TargettedLimb.Body;
            power.Person = Limb.Person;
            power.Skin = Nanotech ?? Mod.Hulk;

            foreach (var limb in Person.Limbs)
            {
                power.threshold.Add(limb, limb.BreakingThreshold);

                Sprite clonedSprite = Functions.Clone(limb.PhysicalBehaviour.spriteRenderer.sprite);
                clonedSprite.name = limb.name;
                power.Sprites.Add(clonedSprite);

                if (limb.Joint)
                    limb.Joint.autoConfigureConnectedAnchor = false;

                limb.BreakingThreshold = Mathf.Infinity;

                if (limb.name.Contains("LowerArm"))
                {
                    SuperPunch.SetPower(Person, limb, null);
                }
            }

            return power;
        }

        public void Use2()
        {
            if (transformed)
                TransformBack();
            else if (Enabled)
                Transform();
        }

        public void FixedUpdate()
        {
            if (TransformOnceNoAnim)
                if (!Person.GetComponentInChildren<SpriteMergerAnimatorAdvanced>())
                {
                    TransformOnceNoAnim = false;
                    Use2();
                }

            if (Enabled && !transformed)
                if (!Person.IsAlive())
                    Transform();
        }

        public void Transform()
        {
            if (Person.GetComponentInChildren<SpriteMergerAnimatorAdvanced>())
                return;

            foreach (var power in transform.root.GetComponentsInChildren<Power>())
            {
                if (power != this && power.Enabled)
                    power.DisablePower();
            }

            if (Person.GetComponentInChildren<SpriteMergerAnimatorAdvanced>() || !Enabled)
            {
                TransformOnceNoAnim = true;
                return;
            }

            transformed = true;

            foreach (var limb in Person.Limbs)
            {
                StartCoroutine(SmoothScale(limb.transform, new Vector2(1.3f, 1.3f)));

                limb.PhysicalBehaviour.Properties = ModAPI.FindPhysicalProperties("Incredible");
                limb.PhysicalBehaviour.BulletPenetration = false;
                limb.PhysicalBehaviour.Properties.BulletSpeedAbsorptionPower = 1;
                limb.ImpactDamageMultiplier = 0.001f;
                limb.ImpactPainMultiplier = 0;
                limb.ShotDamageMultiplier = 0;
                limb.BreakingThreshold = Mathf.Infinity;
                limb.ImmuneToDamage = true;
                limb.CirculationBehaviour.ImmuneToDamage = true;
            }

            Sprites.Clear();
            foreach (var limb in Person.Limbs)
            {
                Sprite clonedSprite = Functions.Clone(limb.PhysicalBehaviour.spriteRenderer.sprite);
                clonedSprite.name = limb.name;
                Sprites.Add(clonedSprite);

                if (limb.GetComponent<LineRenderer>())
                {
                    limb.GetComponent<LineRenderer>().startColor = new Color(0, 0, 0, 0);
                    limb.GetComponent<LineRenderer>().endColor = new Color(0, 0, 0, 0);
                }
            }

            foreach (var limb in Person.Limbs)
            {
                limb.gameObject.AddComponent<SpriteMergerAnimatorAdvanced>().Initialize(limb.GetComponent<SpriteRenderer>().sprite, Timtam.GetLimbSprite(Skin, limb), limb.GetComponent<SpriteRenderer>().material, true, 3, AnimationType.Random);

                if (limb.name.Contains("LowerArm"))
                {
                    limb.GetComponent<SuperPunch>().EnablePower();
                }

                if (Person.TryGetComponent<SpeedHealing>(out var heal))
                {
                    if (heal.Enabled)
                        hadHealing = true;
                    else
                    {
                        hadHealing = false;
                        heal.immortal = true;
                        heal.EnablePower();
                    }
                }
                else
                {
                    hadHealing = false;
                    SpeedHealing.SetPower(Person, null).EnablePower();
                }

                if (Person.TryGetComponent<SuperMass>(out var mass))
                {
                    if (mass.Enabled)
                        hadStrength = true;
                    else
                    {
                        hadStrength = false;
                        mass.EnablePower();
                    }
                }
                else
                {
                    hadStrength = false;
                    SuperMass.SetPower(Person, null, 1).EnablePower();
                }

                //Person.Limbs[2].PhysicalBehaviour.PlayClipOnce(TransformationSound);
            }
        }

        public void TransformBack()
        {
            if (Person.GetComponentInChildren<SpriteMergerAnimatorAdvanced>())
                return;

            foreach (var power in transform.root.GetComponentsInChildren<Power>())
            {
                if (power != this && power.Enabled)
                    power.DisablePower();
            }

            if (Person.GetComponentInChildren<SpriteMergerAnimatorAdvanced>())
            {
                TransformOnceNoAnim = true;
                return;
            }

            transformed = false;

            foreach (var limb in Person.Limbs)
            {
                StartCoroutine(SmoothScale(limb.transform, new Vector2(1f, 1f)));
                limb.PhysicalBehaviour.Properties = ModAPI.FindPhysicalProperties("Human");
                limb.PhysicalBehaviour.BulletPenetration = true;
                limb.ImpactDamageMultiplier = 1;
                limb.ImpactPainMultiplier = 1;
                limb.ShotDamageMultiplier = 1;
                limb.BreakingThreshold = Mathf.Infinity;
                limb.ImmuneToDamage = false;
            }

            //Person.Limbs[2].PhysicalBehaviour.PlayClipOnce(TransformationSound);

            foreach (var limb in GetDeepestPushedToLimbs(Person.Limbs[1]))
            {
                Timtam.MakeCustomSkinSpread(limb, Sprites, false, true, 2, true, Sprites, 2, true);

                if (limb.TryGetComponent<LineRenderer>(out var line))
                {
                    line.startColor = Color.white;
                    line.endColor = Color.white;
                }

            }

            if (!hadHealing)
                Person.GetComponent<SpeedHealing>().DisablePower();

            if (!hadStrength)
                Person.GetComponent<SuperMass>().DisablePower();
        }

        public static List<LimbBehaviour> GetDeepestPushedToLimbs(LimbBehaviour rootLimb)
        {
            var result = new List<LimbBehaviour>();
            var visited = new HashSet<CirculationBehaviour>();

            void Traverse(CirculationBehaviour cb)
            {
                if (cb == null || visited.Contains(cb))
                    return;

                visited.Add(cb);

                if (cb.PushesTo == null || cb.PushesTo.Count() == 0)
                {
                    if (cb.Limb != null)
                        result.Add(cb.Limb);
                    return;
                }

                foreach (var nextCb in cb.PushesTo)
                {
                    Traverse(nextCb);
                }
            }

            if (rootLimb?.CirculationBehaviour != null)
                Traverse(rootLimb.CirculationBehaviour);

            return result;
        }

        public IEnumerator SmoothScale(Transform Target, Vector2 targetScale)
        {
            Vector2 initialScale = Target.localScale;
            float elapsed = 0f;
            float stretchDuration = 1f;

            while (elapsed < stretchDuration)
            {
                float t = elapsed / stretchDuration;
                Target.localScale = Vector2.Lerp(initialScale, targetScale, t);

                var phys = Target.GetComponent<PhysicalBehaviour>();
                if (phys)
                {
                    phys.RecalculateMassBasedOnSize();
                }

                elapsed += Time.deltaTime;
                yield return null;
            }

            Target.localScale = targetScale;

            foreach (var sizeChange in transform.root.GetComponentsInChildren<SizeChange>())
            {
                sizeChange.stretching = false;
            }
        }
    }

    public class SizeChange : Power, Messages.IUse
    {
        float defaultSize = 3;
        public float Size = 3;
        public bool Stretched = false;
        float stretchDuration = 1f;
        public bool stretching;

        public static SizeChange SetPower(PersonBehaviour person, LimbBehaviour limb, Sprite icon, float size = 0.1f, string name = "Shrink")
        {
            var power = limb.gameObject.AddComponent<SizeChange>();
            power.icon = icon;
            power.Name = name;
            power.Description = name+"s the user";
            power.targetLimb = Mod.ModAPIPlus.GetTargettedLimb(limb.gameObject);
            power.Size = size;
            power.defaultSize = size;

            limb.PhysicalBehaviour.ContextMenuOptions.Buttons.Add(new ContextMenuButton(() => !ColorpickerDialogBehaviour.IsOpen, "setAddSizeThing", "Change Size", "Change Size", delegate
            {
                Utils.OpenFloatInputDialog(power.Size, power, delegate (SizeChange obj, float c)
                {
                    power.Size = c;

                }, "Change " + name + " Size", "Change the target size of the user's ability (Default value is" + power.defaultSize + ")");
            }));

            foreach (var Limb in person.Limbs)
                if (Limb.Joint != null)
                    Limb.Joint.autoConfigureConnectedAnchor = false;
            return power;
        }

        public void Use(ActivationPropagation activation)
        {
            if (!Enabled || stretching)
                return;

            if (Stretched)
            {
                foreach (var sizeChange in transform.root.GetComponentsInChildren<SizeChange>())
                {
                    sizeChange.stretching = true;
                }
                foreach (var limb in GetComponent<LimbBehaviour>().Person.Limbs)
                {
                    ModAPI.CreateParticleEffect("Flash", limb.transform.position);
                    StartCoroutine(SmoothScale(limb.transform, new Vector2(1, 1)));
                }
                foreach (var sizeChange in transform.root.GetComponentsInChildren<SizeChange>())
                {
                    sizeChange.Stretched = false;
                }

                transform.root.GetComponent<PersonBehaviour>().Limbs[2].PhysicalBehaviour.PlayClipOnce(Mod.Shrink);
            }
            else
            {
                foreach (var sizeChange in transform.root.GetComponentsInChildren<SizeChange>())
                {
                    sizeChange.stretching = true;
                }
                foreach (var limb in GetComponent<LimbBehaviour>().Person.Limbs)
                {
                    ModAPI.CreateParticleEffect("Spark", limb.transform.position);
                    StartCoroutine(SmoothScale(limb.transform, new Vector2(Size, Size)));
                }
                foreach (var sizeChange in transform.root.GetComponentsInChildren<SizeChange>())
                {
                    sizeChange.Stretched = true;
                }

                transform.root.GetComponent<PersonBehaviour>().Limbs[2].PhysicalBehaviour.PlayClipOnce(Mod.Shrink);
            }
        }

        public IEnumerator SmoothScale(Transform Target, Vector2 targetScale)
        {
            Vector2 initialScale = Target.localScale;
            float elapsed = 0f;

            float[] thresholds = { 0.25f, 0.50f, 0.75f, 1.0f };
            int nextThresholdIndex = 0;

            while (elapsed < stretchDuration)
            {
                float t = elapsed / stretchDuration;
                Target.localScale = Vector2.Lerp(initialScale, targetScale, t);

                var phys = Target.GetComponent<PhysicalBehaviour>();
                if (phys)
                {
                    phys.RecalculateMassBasedOnSize();
                }

                while (nextThresholdIndex < thresholds.Length && t >= thresholds[nextThresholdIndex])
                {
                    StartCoroutine(CreateAfterimage(Target));
                    nextThresholdIndex++;
                }

                elapsed += Time.deltaTime;
                yield return null;
            }

            Target.localScale = targetScale;
            if (nextThresholdIndex < thresholds.Length)
            {
                StartCoroutine(CreateAfterimage(Target));
            }

            foreach (var sizeChange in transform.root.GetComponentsInChildren<SizeChange>())
            {
                sizeChange.stretching = false;
            }
        }

        private IEnumerator CreateAfterimage(Transform target)
        {
            if (!target) yield break;

            var sr = target.GetComponent<SpriteRenderer>();
            if (sr == null || sr.sprite == null) yield break;
            
            GameObject afterimage = new GameObject("Afterimage");
            afterimage.transform.position = target.position;
            afterimage.transform.rotation = target.rotation;
            var a = target.transform.localScale;
            var b = target.transform.root.localScale;
            afterimage.transform.localScale = new Vector3(a.x * b.x, a.y * b.y, 1);

            var aisr = afterimage.AddComponent<SpriteRenderer>();
            aisr.sprite = sr.sprite;
            aisr.flipX = sr.flipX;
            aisr.flipY = sr.flipY;
            aisr.sortingLayerID = sr.sortingLayerID;
            aisr.sortingOrder = sr.sortingOrder - 1;
            aisr.material = sr.sharedMaterial;

            Color baseColor = sr.color;
            baseColor.a = 0.9f;
            aisr.color = baseColor;

            yield return StartCoroutine(FadeAndDestroy(aisr, 2f));
        }

        private IEnumerator FadeAndDestroy(SpriteRenderer sr, float duration)
        {
            if (!sr) yield break;
            float elapsed = 0f;
            Color start = sr.color;
            while (elapsed < duration && sr)
            {
                float t = elapsed / duration;
                if (sr)
                {
                    Color c = start;
                    c.a = Mathf.Lerp(start.a, 0f, t);
                    sr.color = c;
                }
                elapsed += Time.deltaTime;
                yield return null;
            }
            if (sr)
            {
                Color c = sr.color;
                c.a = 0f;
                sr.color = c;
                Destroy(sr.gameObject);
            }
        }
    }

    public class TBLiquid : Liquid
    {
        public const string ID = "THANOSBLOOD";
        public override string GetDisplayName() => "Titanian Blood";

        public TBLiquid()
        {
            Color = new UnityEngine.Color32(138, 52, 123, 255);
        }

        public override void OnEnterContainer(BloodContainer container)
        {

        }

        public override void OnEnterLimb(LimbBehaviour limb)
        {

        }

        public override void OnUpdate(BloodContainer c)
        {
            base.OnUpdate(c);
            if (c is CirculationBehaviour circ)
            {
                var limb = circ.Limb;

                if (limb.SpeciesIdentity == Species.Android)
                    return;

                limb.CirculationBehaviour.HealBleeding();
            }
        }

        public override void OnExitContainer(BloodContainer container)
        {

        }
    }

    public class SpiderArm : MonoBehaviour
    {
        public List<PhysicalBehaviour> Segments = new List<PhysicalBehaviour>();
        Vector2 connectionOffset = Vector2.zero;

        public static SpiderArm SetArm(
            PhysicalBehaviour Connected,
            List<Sprite> segmentSprites,
            Vector2 connectionOffset,
            float angleLimitation,
            float motorTorque = 5000f,
            bool stiff = true)
        {
            if (Connected == null || segmentSprites == null || segmentSprites.Count == 0)
            {
                Debug.LogWarning("[SpiderArm] Invalid arguments.");
                return null;
            }

            var arm = Connected.gameObject.AddComponent<SpiderArm>();

            var root = Connected.transform.root;
            var baseColliders = root.GetComponentsInChildren<Collider2D>(true).ToList();
            float pixelScale = ModAPI.PixelSize;
            int pixelLength = segmentSprites[0].texture.width;
            float segmentWorldLength = pixelLength * pixelScale;

            Vector3 baseAnchorWorld = Connected.transform.TransformPoint(connectionOffset * pixelScale);

            Vector3 visualRight = Connected.transform.root.localScale.x < 0 ? -Connected.transform.right : Connected.transform.right;
            
            Vector3 buildDir = -visualRight.normalized;

            var createdSegments = new List<PhysicalBehaviour>();

            for (int i = 0; i < segmentSprites.Count; i++)
            {
                Sprite segSprite = segmentSprites[i];
                if (!segSprite) continue;

                var segmentGO = ModAPI.CreatePhysicalObject("SpiderArmSegment_" + i, segSprite);
                var phys = segmentGO.GetComponent<PhysicalBehaviour>();
                createdSegments.Add(phys);
                Destroy(segmentGO.GetComponent<Collider2D>());
                segmentGO.AddComponent<PolygonCollider2D>();
                Timtam.CreateCollider(segmentGO.GetComponent<SpriteRenderer>());
                var rb = phys.rigidbody;
                rb.interpolation = RigidbodyInterpolation2D.Interpolate;
                rb.gravityScale = 0f;
                rb.drag = 5f;
                rb.angularDrag = 5f;
                rb.mass = Connected.rigidbody.mass * 0.3f;
                phys.TrueInitialMass = rb.mass;
                segmentGO.AddComponent<Dampener>().arm = arm;
                segmentGO.transform.parent = Connected.transform.parent;

                if (Connected.transform.root.TryGetComponent<DisintegrationCounterBehaviour>(out var des))
                    Destroy(des);

                Vector3 centerPos = baseAnchorWorld + buildDir * ((i + 0.5f) * segmentWorldLength);
                segmentGO.transform.position = centerPos;

                float angle = Mathf.Atan2(buildDir.y, buildDir.x) * Mathf.Rad2Deg;
                segmentGO.transform.rotation = Quaternion.Euler(0, 0, angle);

                HingeJoint2D hinge = segmentGO.AddComponent<HingeJoint2D>();
                hinge.autoConfigureConnectedAnchor = false;
                hinge.enableCollision = false;
                hinge.useLimits = angleLimitation > 0.01f;

                hinge.anchor = new Vector2(-segmentWorldLength / 2f, 0f);

                if (i == 0)
                {
                    hinge.connectedBody = Connected.rigidbody;
                    hinge.connectedAnchor = Connected.rigidbody.transform.InverseTransformPoint(baseAnchorWorld);
                }
                else
                {
                    var prev = createdSegments[i - 1];
                    hinge.connectedBody = prev.rigidbody;

                    Vector3 prevOutWorld = prev.transform.position + prev.transform.right * (segmentWorldLength / 2f);
                    hinge.connectedAnchor = prev.rigidbody.transform.InverseTransformPoint(prevOutWorld);
                }

                if (i == segmentSprites.Count - 1)
                {
                    var newp = Instantiate(phys.Properties);
                    newp.Sharp = true;
                    newp.SharpAxes = new SharpAxis[] { new SharpAxis(Vector2.right, -0.26f, 0.28f, true, true) };
                    phys.Properties = newp;
                    Debug.Log("[SpiderArm] Added sharpness to end segment.");
                }

                if (hinge.useLimits)
                {
                    JointAngleLimits2D limits = new JointAngleLimits2D
                    {
                        min = -angleLimitation,
                        max = angleLimitation
                    };
                    hinge.limits = limits;
                }

                if (stiff && motorTorque > 0f)
                {
                    hinge.useMotor = true;
                    var motor = hinge.motor;
                    motor.motorSpeed = 0f;
                    motor.maxMotorTorque = motorTorque;
                    hinge.motor = motor;
                }

                
            }

            foreach (var seg in createdSegments)
            {
                var segCols = seg.GetComponentsInChildren<Collider2D>(true);
                foreach (var sc in segCols)
                {
                    foreach (var baseCol in baseColliders)
                    {
                        if (baseCol && sc) Physics2D.IgnoreCollision(sc, baseCol, true);
                    }
                }
            }

            for (int i = 0; i < createdSegments.Count; i++)
            {
                var colsA = createdSegments[i].GetComponentsInChildren<Collider2D>(true);
                for (int j = i + 1; j < createdSegments.Count; j++)
                {
                    var colsB = createdSegments[j].GetComponentsInChildren<Collider2D>(true);
                    foreach (var ca in colsA)
                    {
                        foreach (var cb in colsB)
                        {
                            if (ca && cb) Physics2D.IgnoreCollision(ca, cb, true);
                        }
                    }
                }
            }

            arm.Segments = createdSegments;
            arm.connectionOffset = connectionOffset;
            return arm;
        }

        public void Reposition()
        {
            float pixelScale = ModAPI.PixelSize;
            Vector3 baseAnchorWorld = transform.TransformPoint(connectionOffset * pixelScale);

            Vector3 visualRight = transform.root.localScale.x < 0 ? -transform.right : transform.right;

            Vector3 buildDir = -visualRight.normalized;

            int pixelLength = Segments[0].GetComponent<SpriteRenderer>().sprite.texture.width;
            float segmentWorldLength = pixelLength * pixelScale;

            for (int i = 0; i < Segments.Count; i++)
            {
                var seg = Segments[i];

                Vector3 centerPos = baseAnchorWorld + buildDir * ((i + 0.5f) * segmentWorldLength);
                seg.transform.position = centerPos;

                float angle = Mathf.Atan2(buildDir.y, buildDir.x) * Mathf.Rad2Deg;
                seg.transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }

        public class Dampener : MonoBehaviour
        {
            public SpiderArm arm;
            float dampenStrength = 0.00025f;
            float originalMotor;
            JointMotor2D og;
            JointMotor2D dampened;

            public void Start()
            {
                if (TryGetComponent<HingeJoint2D>(out var joint) && joint.useMotor)
                {
                    originalMotor = joint.motor.maxMotorTorque;
                }
                dampened = new JointMotor2D { motorSpeed = 0, maxMotorTorque = originalMotor * dampenStrength };
                og = new JointMotor2D { motorSpeed = 0, maxMotorTorque = originalMotor };
            }

            public void Update()
            {
                if (SelectionController.Main.SelectedObjects.Contains(GetComponent<PhysicalBehaviour>()))
                    foreach (var seg in arm.Segments)
                        seg.GetComponent<HingeJoint2D>().motor = dampened;
                else
                    foreach (var seg in arm.Segments)
                        seg.GetComponent<HingeJoint2D>().motor = og;
            }
        }
    }

    public class MechArms : Power, Messages.IUse
    {
        bool armsOut = true;

        public static MechArms SetPower(PersonBehaviour person, LimbBehaviour limb, Sprite icon)
        {
            var power = limb.gameObject.AddComponent<MechArms>();
            power.icon = icon;
            power.Name = "Spider Arms";
            power.Description = "Gives the user 4 spider arms on their back, toggleable by activating middle body";
            power.targetLimb = Mod.ModAPIPlus.GetTargettedLimb(limb.gameObject);
            return power;
        }

        public override void DisablePower()
        {
            base.DisablePower();
            if (armsOut)
            {
                foreach (var spiderarm in transform.root.GetComponentsInChildren<SpiderArm>())
                {
                    foreach (var seg in spiderarm.Segments)
                    {
                        seg.enabled = false;
                    }
                }
            }
        }

        public void Use(ActivationPropagation activation)
        {
            if (!Enabled)
                return;

            if (armsOut)
            {
                foreach (var spiderarm in transform.root.GetComponentsInChildren<SpiderArm>())
                {
                    foreach (var seg in spiderarm.Segments)
                    {
                        seg.gameObject.SetActive(false);
                    }
                }
                armsOut = false;
            }
            else
            {
                foreach (var spiderarm2 in transform.root.GetComponentsInChildren<SpiderArm>())
                {
                    spiderarm2.Reposition();
                    foreach (var seg in spiderarm2.Segments)
                    {
                        seg.gameObject.SetActive(true);
                    }
                }

                foreach (var col1 in transform.root.GetComponentsInChildren<Collider2D>())
                {
                    foreach (var col2 in transform.root.GetComponentsInChildren<Collider2D>())
                    {
                        Physics2D.IgnoreCollision(col1, col2, true);
                    }
                }
                armsOut = true;
            }
        }

    }

    public class Thruster : Power, Messages.IUse
    {
        public bool InFlight = false;

        public PersonBehaviour person;

        GameObject SpriteLightFlash;

        float gravity;
        float ogbasestrength = 8.5f;

        public Color32 ThrustColor = new Color32(52, 204, 255, 255);
        public Color32 ThrustColor2 = new Color32(96, 210, 255, 20);

        public static Thruster SetPower(PersonBehaviour person, LimbBehaviour limb, Sprite icon, Color32 thrust = new Color32(), Color32 thrust2 = new Color32())
        {
            var power = limb.gameObject.AddComponent<Thruster>();
            power.icon = icon;
            power.person = person;
            power.Name = "Thruster";
            power.Description = "Allows the user to fly by activating their feet!";
            power.targetLimb = Mod.ModAPIPlus.GetTargettedLimb(limb.gameObject);
            power.ThrustColor = thrust.CompareRGB(new Color32()) ? power.ThrustColor : thrust;
            power.ThrustColor2 = thrust2.CompareRGB(new Color32()) ? power.ThrustColor2 : thrust2;
            power.targetLimb = Mod.ModAPIPlus.GetTargettedLimb(limb.gameObject);
            return power;
        }

        void OnDestroy()
        {
            Destroy(SpriteLightFlash);
        }

        public override void DisablePower()
        {
            base.DisablePower();
            StopThrusting();
        }

        public override void Start()
        {
            base.Start();
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
            SpriteLightFlash.GetComponent<ParticleSystem>().startColor = ThrustColor;
            SpriteLightFlash.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().startColor = ThrustColor2;
            SpriteLightFlash.transform.parent = transform;
            SpriteLightFlash.transform.localPosition = Vector2.zero;
            if (person.transform.localScale.x > 0)
            {
                SpriteLightFlash.transform.localRotation = Quaternion.Euler(0, 0, 90);
            }
            else
            {
                SpriteLightFlash.transform.localRotation = Quaternion.Euler(0, 0, -90);
            }

            SpriteLightFlash.transform.localScale = new Vector3(0.5f, 0.3f, 0.4f);
        }

        public void FixedUpdate()
        {
            if (InFlight)
            {
                foreach (var limb in person.Limbs)
                {
                    if (limb.GetComponent<Rigidbody2D>().bodyType != RigidbodyType2D.Static)
                    {
                        limb.GetComponent<Rigidbody2D>().angularVelocity *= 0.94f;
                        limb.GetComponent<Rigidbody2D>().velocity *= 0.94f;

                        if (limb.name.Contains("Body"))
                        {
                            float num2 = limb.gameObject.GetComponent<PhysicalBehaviour>().rigidbody.mass / 1.5f;
                            float num3 = 10 * Mathf.Clamp(limb.gameObject.GetComponent<PhysicalBehaviour>().Charge, 1f, 5f) * num2 * num2;
                            limb.gameObject.GetComponent<PhysicalBehaviour>().rigidbody.angularVelocity *= Mathf.Pow(0.5f, 1f);
                            limb.gameObject.GetComponent<Rigidbody2D>().AddTorque(Mathf.DeltaAngle(limb.transform.eulerAngles.z, 0f) * num3);
                        }
                    }
                }
            }
        }

        public void StopThrusting()
        {
            if (!InFlight)
                return;

            InFlight = false;
            SpriteLightFlash.GetComponent<ParticleSystem>().Stop();
            person.OverridePoseIndex = -1;
            foreach (var limb in person.Limbs)
            {
                if (limb.name.Contains("Arm"))
                {
                    limb.BaseStrength = ogbasestrength;
                }

                if (limb.TryGetComponent<Thruster>(out var thrus) && thrus != this)
                        thrus.StopThrusting();

                limb.GetComponent<Rigidbody2D>().gravityScale = gravity;
            }
        }

        public void StartThrusting()
        {
            if (!InFlight && Enabled)
            {
                person.OverridePoseIndex = 8;
                InFlight = true;
                SpriteLightFlash.GetComponent<ParticleSystem>().Play();
                foreach (var limb in person.Limbs)
                {
                    if (limb.name.Contains("Arm"))
                    {
                        limb.BaseStrength = 0;
                    }
                    gravity = limb.GetComponent<PhysicalBehaviour>().InitialGravityScale;
                    limb.GetComponent<Rigidbody2D>().gravityScale = 0;
                    if (limb.TryGetComponent<Thruster>(out var thrus) && thrus != this)
                        thrus.StartThrusting();
                }
            }
        }

        public void Use(ActivationPropagation activation)
        {
            if (!Enabled)
                return;

            if (!InFlight)
            {
                StartThrusting();
            }
            else
            {
                StopThrusting();
            }
        }
    }

    public class ChestRepulsor : Power
    {
        LimbBehaviour limb;
        public Color32 laserColor = new Color32(0, 255, 250, 255);

        public static ChestRepulsor SetPower(PersonBehaviour person, LimbBehaviour limb, Sprite icon, Color32 laserColor = new Color32())
        {
            var power = limb.gameObject.AddComponent<ChestRepulsor>();
            power.limb = limb;
            power.icon = icon;
            power.Name = "Chest Repulsor";
            power.Description = "Shoots a powerful energy blast out of the chest.";
            power.targetLimb = Mod.ModAPIPlus.GetTargettedLimb(limb.gameObject);
            power.laserColor = laserColor.CompareRGB(new Color32()) ? power.laserColor : laserColor;
            return power;
        }

        public override void EnablePower()
        {
            base.EnablePower();

            var TargetPerson = limb.Person;
            foreach (var blast in limb.GetComponents<BlasterBehaviour>())
            {
                Destroy(blast.Muzzleflash.gameObject);
                Destroy(blast.blasterSoundSource);
                Destroy(blast);
            }
            if (limb.GetComponent<AudioDistortionFilter>())
            {
                Destroy(GetComponent<AudioDistortionFilter>());
            }
            if (limb.GetComponent<AudioSourceTimeScaleBehaviour>())
            {
                Destroy(GetComponent<AudioSourceTimeScaleBehaviour>());
            }

            var blaster = gameObject.AddComponent<BlasterBehaviour>();
            var prefGun = ModAPI.FindSpawnable("Blaster").Prefab.GetComponent<BlasterBehaviour>();

            if (TargetPerson.transform.localScale.x > 0)
            {
                blaster.barrelDirection = Vector2.right;
                blaster.barrelPosition = new Vector2(0.2f, 0);
            }
            else
            {
                blaster.barrelDirection = Vector2.right;
                blaster.barrelPosition = new Vector2(0.2f, 0);
            }
            var audiosource = gameObject.AddComponent<AudioSource>();
            audiosource.volume = 0.5f;
            audiosource.spatialBlend = 1;
            Global.main.AddAudioSource(audiosource, false);
            if (!GetComponent<AudioDistortionFilter>())
            {
                gameObject.AddComponent<AudioDistortionFilter>().distortionLevel = 0.85f;
            }

            if (!GetComponent<AudioSourceTimeScaleBehaviour>())
            {
                gameObject.AddComponent<AudioSourceTimeScaleBehaviour>();
            }

            blaster.blasterSoundSource = audiosource;
            blaster.Recoil = 1;

            blaster.Bolt = Instantiate(ModAPI.FindSpawnable("Detached Laser Cannon").Prefab.GetComponent<BlasterBehaviour>().Bolt);
            blaster.Bolt.GetComponent<BlasterboltBehaviour>().Trail.startColor = ModAPI.FindSpawnable("Blaster Rifle").Prefab.GetComponent<BlasterBehaviour>().Bolt.GetComponent<TrailRenderer>().startColor;

            blaster.Bolt.GetComponent<BlasterboltBehaviour>().Trail.endColor = Color.white;

            Destroy(blaster.Bolt.transform.FindChild("glow2").gameObject);

            var muzzleflash = Instantiate(ModAPI.FindSpawnable("Blaster Rifle").Prefab.transform.GetChild(0).gameObject);
            muzzleflash.transform.parent = limb.transform;
            muzzleflash.transform.localRotation = Quaternion.Euler(0, 0, 0);
            muzzleflash.transform.localPosition = new Vector2(0.1f, 0);
            muzzleflash.GetComponent<ParticleSystem>().startColor = laserColor;
            muzzleflash.transform.GetChild(0).GetComponent<ParticleSystem>().startColor = laserColor;
            if (GetComponent<LimbBehaviour>().Person.transform.localScale.x < 0)
            {
                muzzleflash.transform.localRotation = Quaternion.Euler(0, 0, 180);
                muzzleflash.transform.localScale = new Vector2(0.8703f, 0.8703f);
            }

            blaster.Muzzleflash = muzzleflash.GetComponent<ParticleSystem>();

            blaster.Bolt.transform.position = new Vector3(999990f, 999990f, 999990f);
            blaster.Bolt.GetComponent<BlasterboltBehaviour>().Trail.startColor = laserColor;
            blaster.Bolt.GetComponent<BlasterboltBehaviour>().Trail.endColor = laserColor;
            blaster.Bolt.GetComponent<BlasterboltBehaviour>().Trail.widthMultiplier = +0.2f;
            blaster.Bolt.GetComponent<BlasterboltBehaviour>().ImpactStrength = 0.1f;
            blaster.Bolt.GetComponent<BlasterboltBehaviour>().damage = 10;

            List<AudioClip> clips = new List<AudioClip>();
            clips.Add(Mod.ArcBlast2);
            blaster.Clips = clips.ToArray();
        }

        public override void DisablePower()
        {
            base.DisablePower();
            foreach (var blast in limb.GetComponents<BlasterBehaviour>())
            {
                Destroy(blast.Muzzleflash.gameObject);
                Destroy(blast.blasterSoundSource);
                Destroy(blast);
            }
            if (limb.GetComponent<AudioDistortionFilter>())
            {
                Destroy(limb.GetComponent<AudioDistortionFilter>());
            }
            if (limb.GetComponent<AudioSourceTimeScaleBehaviour>())
            {
                Destroy(limb.GetComponent<AudioSourceTimeScaleBehaviour>());
            }
        }
    }

    public class Repulsor : Power
    {
        LimbBehaviour limb;
        public Vector2 BarrelPos = new Vector2(0, -0.25f);
        public Color32 laserColor = new Color(0, 255, 250, 255);

        public static Repulsor SetPower(PersonBehaviour person, LimbBehaviour limb, Sprite icon, Color32 laserColor = new Color32())
        {
            var power = limb.gameObject.AddComponent<Repulsor>();
            power.limb = limb;
            power.icon = icon;
            power.Name = "Repulsor";
            power.Description = "Shoots a powerful energy blast.";
            power.targetLimb = Mod.ModAPIPlus.GetTargettedLimb(limb.gameObject);
            power.laserColor = laserColor.CompareRGB(new Color32()) ? power.laserColor : laserColor;
            return power;
        }

        public override void EnablePower()
        {
            base.EnablePower();

            var TargetPerson = limb.Person;

            foreach (var blast in limb.GetComponents<BlasterBehaviour>())
            {
                Destroy(blast.Muzzleflash.gameObject);
                Destroy(blast.blasterSoundSource);
                Destroy(blast);
            }

            if (limb.GetComponent<AudioDistortionFilter>())
            {
                Destroy(limb.GetComponent<AudioDistortionFilter>());
            }
            if (limb.GetComponent<AudioSourceTimeScaleBehaviour>())
            {
                Destroy(limb.GetComponent<AudioSourceTimeScaleBehaviour>());
            }

            var blaster = gameObject.AddComponent<BlasterBehaviour>();
            var prefGun = ModAPI.FindSpawnable("Blaster").Prefab.GetComponent<BlasterBehaviour>();

            blaster.barrelDirection = Vector2.down;

            blaster.barrelPosition = BarrelPos;

            var audiosource = gameObject.AddComponent<AudioSource>();
            audiosource.volume = 0.5f;
            audiosource.spatialBlend = 1;
            Global.main.AddAudioSource(audiosource, false);
            if (!GetComponent<AudioDistortionFilter>())
            {
                gameObject.AddComponent<AudioDistortionFilter>().distortionLevel = 0.85f;
            }

            if (!GetComponent<AudioSourceTimeScaleBehaviour>())
            {
                gameObject.AddComponent<AudioSourceTimeScaleBehaviour>();
            }
            blaster.blasterSoundSource = audiosource;
            blaster.Recoil = 1;

            blaster.Bolt = UnityEngine.Object.Instantiate(ModAPI.FindSpawnable("Blaster Rifle").Prefab.GetComponent<BlasterBehaviour>().Bolt);

            var muzzleflash = UnityEngine.Object.Instantiate(ModAPI.FindSpawnable("Blaster Rifle").Prefab.transform.GetChild(0).gameObject);
            muzzleflash.transform.parent = limb.transform;
            muzzleflash.transform.localRotation = Quaternion.Euler(0, 0, 270);
            muzzleflash.transform.localPosition = new Vector2(0, -0.3f);
            muzzleflash.GetComponent<ParticleSystem>().startColor = laserColor;
            muzzleflash.transform.GetChild(0).GetComponent<ParticleSystem>().startColor = laserColor;
            if (GetComponent<LimbBehaviour>().Person.transform.localScale.x < 0)
            {
                muzzleflash.transform.localRotation = Quaternion.Euler(0, 0, 90);
                muzzleflash.transform.localScale = new Vector2(0.8703f, 0.8703f);
            }
            blaster.Muzzleflash = muzzleflash.GetComponent<ParticleSystem>();

            blaster.Bolt.transform.position = new Vector3(999990f, 999990f, 999990f);
            blaster.Bolt.GetComponent<BlasterboltBehaviour>().Trail.startColor = laserColor;
            blaster.Bolt.GetComponent<BlasterboltBehaviour>().Trail.endColor = laserColor;
            blaster.Bolt.GetComponent<BlasterboltBehaviour>().Trail.widthMultiplier = +0.2f;
            blaster.Bolt.GetComponent<BlasterboltBehaviour>().ImpactStrength = 0.1f;
            blaster.Bolt.GetComponent<BlasterboltBehaviour>().damage = 10;
            List<AudioClip> clips = new List<AudioClip>();

            clips.Add(Mod.ArcBlast);

            blaster.Clips = clips.ToArray();
        }

        public override void DisablePower()
        {
            base.DisablePower();
            foreach (var blast in limb.GetComponents<BlasterBehaviour>())
            {
                Destroy(blast.Muzzleflash.gameObject);
                Destroy(blast.blasterSoundSource);
                Destroy(blast);
            }
            if (limb.GetComponent<AudioDistortionFilter>())
            {
                Destroy(limb.GetComponent<AudioDistortionFilter>());
            }
            if (limb.GetComponent<AudioSourceTimeScaleBehaviour>())
            {
                Destroy(limb.GetComponent<AudioSourceTimeScaleBehaviour>());
            }
        }
    }

    public class RepulsorCannons : Power, Mod.ModAPIPlus.IUse2
    {
        public bool usingCannon = false;
        public Sprite OGArmSprite;
        public Sprite Repulsor = Mod.RepulsorCannon;

        public static RepulsorCannons SetPower(PersonBehaviour person, LimbBehaviour limb, Sprite icon)
        {
            var power = limb.gameObject.AddComponent<RepulsorCannons>();
            power.icon = icon;
            power.Name = "Repulsor Cannon";
            power.Description = "Shoots a very powerful energy blast. ";
            power.targetLimb = Mod.ModAPIPlus.GetTargettedLimb(limb.gameObject);
            return power;
        }

        public override void DisablePower()
        {
            base.DisablePower();
            UnCannon();
        }

        public void Use2()
        {
            if (GetComponent<SpriteMergerAnimator>() || GetComponent<SpriteMergerAnimatorAdvanced>() || !Enabled)
                return;

            StartCoroutine(PlaySound(Mod.NanoGunTransform));

            if (usingCannon)
            {
                UnCannon();
            }
            else
            {
                Cannon();
            }
        }

        public void Cannon()
        {
            OGArmSprite = Functions.Clone(GetComponent<SpriteRenderer>().sprite);
            
            gameObject.AddComponent<SpriteMergerAnimatorAdvanced>().Initialize(gameObject.GetComponent<SpriteRenderer>().sprite, new List<Sprite> { Timtam.MergeSprites(OGArmSprite, Repulsor), Timtam.MergeSprites(OGArmSprite, Repulsor) }, gameObject.GetComponent<SpriteRenderer>().material, gameObject.GetComponent<SpriteRenderer>().material, 2, AnimationType.BottomToTop, false, 1);

            foreach (var blast in GetComponents<BlasterBehaviour>())
            {
                Destroy(blast.Muzzleflash.gameObject);
                Destroy(blast.blasterSoundSource);
                Destroy(blast);
            }

            var target = gameObject;
            var TargetPerson = gameObject.GetComponent<LimbBehaviour>().Person;
            var blaster = target.AddComponent<BlasterBehaviour>();
            var prefGun = ModAPI.FindSpawnable("Blaster").Prefab.GetComponent<BlasterBehaviour>();

            blaster.barrelDirection = Vector2.down;
            if (TargetPerson.transform.localScale.x > 0)
            {
                blaster.barrelPosition = new Vector2(0f, -0.25f);
            }
            else
            {
                blaster.barrelPosition = new Vector2(0f, -0.25f);
            }

            var audiosource = target.AddComponent<AudioSource>();
            audiosource.volume = 0.5f;
            audiosource.spatialBlend = 1;
            Global.main.AddAudioSource(audiosource, false);
            if (!GetComponent<AudioDistortionFilter>())
            {
                gameObject.AddComponent<AudioDistortionFilter>().distortionLevel = 0.85f;
            }

            if (!GetComponent<AudioSourceTimeScaleBehaviour>())
            {
                gameObject.AddComponent<AudioSourceTimeScaleBehaviour>();
            }

            blaster.blasterSoundSource = audiosource;
            blaster.Recoil = 1;

            blaster.Bolt = UnityEngine.Object.Instantiate(ModAPI.FindSpawnable("Detached Laser Cannon").Prefab.GetComponent<BlasterBehaviour>().Bolt);
            blaster.Bolt.GetComponent<BlasterboltBehaviour>().Trail.startColor = ModAPI.FindSpawnable("Blaster Rifle").Prefab.GetComponent<BlasterBehaviour>().Bolt.GetComponent<TrailRenderer>().startColor;
            var muzzleflash = UnityEngine.Object.Instantiate(ModAPI.FindSpawnable("Blaster Rifle").Prefab.transform.GetChild(0).gameObject);
            muzzleflash.transform.parent = transform;
            muzzleflash.transform.localRotation = Quaternion.Euler(0, 0, 270);
            muzzleflash.transform.localPosition = new Vector2(0, -0.1f);
            if (GetComponent<LimbBehaviour>().Person.transform.localScale.x < 0)
            {
                muzzleflash.transform.localRotation = Quaternion.Euler(0, 0, 90);
                muzzleflash.transform.localScale = new Vector2(0.8703f, 0.8703f);
            }
            blaster.Muzzleflash = muzzleflash.GetComponent<ParticleSystem>();
            blaster.Bolt.GetComponent<BlasterboltBehaviour>().Trail.endColor = Color.white;

            Destroy(blaster.Bolt.transform.FindChild("glow2").gameObject);


            blaster.Bolt.transform.position = new Vector3(999990f, 999990f, 999990f);
            blaster.Bolt.GetComponent<BlasterboltBehaviour>().Trail.widthMultiplier = +0.2f;
            blaster.Bolt.GetComponent<BlasterboltBehaviour>().ImpactStrength = 0.1f;
            blaster.Bolt.GetComponent<BlasterboltBehaviour>().damage = 10;
            List<AudioClip> clips = new List<AudioClip>();
            clips.Add(Mod.ArcBlast2);
            blaster.Clips = clips.ToArray();

            usingCannon = true;
        }

        public void UnCannon()
        {
            if (usingCannon)
            {
                gameObject.AddComponent<SpriteMergerAnimatorAdvanced>().Initialize(gameObject.GetComponent<SpriteRenderer>().sprite, new List<Sprite> { OGArmSprite, OGArmSprite }, gameObject.GetComponent<SpriteRenderer>().material, gameObject.GetComponent<SpriteRenderer>().material, 2, AnimationType.TopToBottom, false, 1);

                foreach (var blast in GetComponents<BlasterBehaviour>())
                {
                    Destroy(blast.Muzzleflash.gameObject);
                    Destroy(blast.blasterSoundSource);
                    Destroy(blast);
                }

                usingCannon = false;
            }
        }

        IEnumerator PlaySound(AudioClip SoundToPlay)
        {
            var sound = new GameObject();
            sound.name = "SFX";
            //sound.transform.parent = transform;
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

    public class NanoBlade : Power, Mod.ModAPIPlus.IUse2
    {
        public bool usingBlade = false;
        public Sprite OGArmSprite;
        public Sprite Nanoblade = Mod.NanoBlade;
        public Vector2 OGBoxColliderSize;
        PhysicalProperties stabhandproperties;

        public static NanoBlade SetPower(PersonBehaviour person, LimbBehaviour limb, Sprite icon, Sprite Nanoblade = null)
        {
            var power = limb.gameObject.AddComponent<NanoBlade>();
            power.icon = icon;
            power.Name = "Nanoblade";
            power.Description = "Creates a dangerously sharp nanotech blade!";
            power.targetLimb = Mod.ModAPIPlus.GetTargettedLimb(limb.gameObject);
            power.Nanoblade = Nanoblade ?? Mod.NanoBlade;
            return power;
        }

        public override void DisablePower()
        {
            base.DisablePower();
            Unblade();
        }

        public override void Start()
        {
            base.Start();
            OGBoxColliderSize = GetComponent<BoxCollider2D>().size;
        }

        public void Use2()
        {
            if (GetComponent<SpriteMergerAnimator>() || GetComponent<SpriteMergerAnimatorAdvanced>() || !Enabled)
                return;
            StartCoroutine(PlaySound(Mod.NanoGunTransform));
            if (usingBlade)
            {
                Unblade();
            }
            else
            {
                Blade();
            }
        }

        public void Blade()
        {
            OGArmSprite = Functions.Clone(GetComponent<SpriteRenderer>().sprite);

            gameObject.AddComponent<SpriteMergerAnimatorAdvanced>().Initialize(gameObject.GetComponent<SpriteRenderer>().sprite, new List<Sprite> { Timtam.MergeSprites(OGArmSprite, Nanoblade), Timtam.MergeSprites(OGArmSprite, Nanoblade) }, gameObject.GetComponent<SpriteRenderer>().material, gameObject.GetComponent<SpriteRenderer>().material, 2, AnimationType.BottomToTop, false, 1);

            foreach (var blast in GetComponents<BlasterBehaviour>())
            {
                Destroy(blast.Muzzleflash.gameObject);
                Destroy(blast.blasterSoundSource);
                Destroy(blast);
            }

            stabhandproperties = Instantiate(ModAPI.FindPhysicalProperties("AndroidArmour"));
            stabhandproperties.Sharp = true;

            stabhandproperties.SharpAxes = new SharpAxis[]
            {
                                new SharpAxis(Vector2.down, -0.74f, 1f, true, true)
            };

            gameObject.GetComponent<LimbBehaviour>().PhysicalBehaviour.Properties = stabhandproperties;

            GetComponent<BoxCollider2D>().size = new Vector2(OGBoxColliderSize.x, OGBoxColliderSize.y + OGBoxColliderSize.y);
            GetComponent<BoxCollider2D>().offset = new Vector2(0, -0.2f);

            usingBlade = true;
        }

        public void Unblade()
        {
            if (usingBlade)
            {
                gameObject.AddComponent<SpriteMergerAnimatorAdvanced>().Initialize(gameObject.GetComponent<SpriteRenderer>().sprite, new List<Sprite> { OGArmSprite, OGArmSprite }, gameObject.GetComponent<SpriteRenderer>().material, gameObject.GetComponent<SpriteRenderer>().material, 2, AnimationType.TopToBottom, false, 1);

                gameObject.GetComponent<PhysicalBehaviour>().Properties = GetComponent<LimbBehaviour>().Person.Limbs[2].PhysicalBehaviour.Properties;
                gameObject.GetComponent<BoxCollider2D>().size = OGBoxColliderSize;
                gameObject.GetComponent<BoxCollider2D>().offset = Vector2.zero;
                usingBlade = false;
            }
        }

        IEnumerator PlaySound(AudioClip SoundToPlay)
        {
            var sound = new GameObject();
            sound.name = "SFX";
            //sound.transform.parent = transform;
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

    public class NanoShield : Power, Mod.ModAPIPlus.IUse2
    {
        public bool usingBlade = false;
        public Sprite OGArmSprite;
        public Sprite Nanoblade = Mod.NanoBlade;
        public BoxCollider2D col;
        public LimbBehaviour Limb;

        public static NanoShield SetPower(PersonBehaviour person, LimbBehaviour limb, Sprite icon, Sprite Nanoshield = null)
        {
            var power = limb.gameObject.AddComponent<NanoShield>();
            power.icon = icon;
            power.Name = "Nano Shield";
            power.Description = "Creates an impenetrable shield";
            power.targetLimb = Mod.ModAPIPlus.GetTargettedLimb(limb.gameObject);
            power.Nanoblade = Nanoshield ?? Mod.NanoShield;
            power.Limb = limb;
            power.col = limb.gameObject.AddComponent<BoxCollider2D>();
            power.col.size = new Vector2(0.145226479f, 1.24006033f);
            power.col.offset = new Vector2(-0.187101364f, -0.0308058262f);
            power.col.enabled = false;
            return power;
        }

        public override void DisablePower()
        {
            base.DisablePower();
            Unblade();
        }

        public override void Start()
        {
            base.Start();
        }

        public void Use2()
        {
            if (GetComponent<SpriteMergerAnimator>() || GetComponent<SpriteMergerAnimatorAdvanced>() || !Enabled)
                return;
            StartCoroutine(PlaySound(Mod.NanoGunTransform));
            if (usingBlade)
            {
                Unblade();
            }
            else
            {
                Blade();
            }
            foreach (var coll in Limb.Person.GetComponentsInChildren<Collider2D>())
            {
                Physics2D.IgnoreCollision(coll, col, true);
            }
        }

        public void Blade()
        {
            OGArmSprite = Functions.Clone(GetComponent<SpriteRenderer>().sprite);

            gameObject.AddComponent<SpriteMergerAnimatorAdvanced>().Initialize(gameObject.GetComponent<SpriteRenderer>().sprite, new List<Sprite> { Timtam.MergeSprites(OGArmSprite, Nanoblade), Timtam.MergeSprites(OGArmSprite, Nanoblade) }, gameObject.GetComponent<SpriteRenderer>().material, gameObject.GetComponent<SpriteRenderer>().material, 2, AnimationType.CircleOut, false, 1);
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<ConnectedNodeBehaviour>().Connections[0].GetComponent<SpriteRenderer>().sortingOrder + 1;
            foreach (var blast in GetComponents<BlasterBehaviour>())
            {
                Destroy(blast.Muzzleflash.gameObject);
                Destroy(blast.blasterSoundSource);
                Destroy(blast);
            }

            col.enabled = true;

            Limb.PhysicalBehaviour.BulletPenetration = false;
            Limb.ShotDamageMultiplier = 0;
            Limb.PhysicalBehaviour.Properties.BulletSpeedAbsorptionPower = 1;
            Limb.PhysicalBehaviour.TrueInitialMass *= 20;
            Limb.BreakingThreshold = Mathf.Infinity;

            usingBlade = true;
        }

        public void Unblade()
        {
            if (usingBlade)
            {
                gameObject.AddComponent<SpriteMergerAnimatorAdvanced>().Initialize(gameObject.GetComponent<SpriteRenderer>().sprite, new List<Sprite> { OGArmSprite, OGArmSprite }, gameObject.GetComponent<SpriteRenderer>().material, gameObject.GetComponent<SpriteRenderer>().material, 2, AnimationType.CircleIn, false, 1);

                col.enabled = false;

                Limb.PhysicalBehaviour.BulletPenetration = false;
                Limb.ShotDamageMultiplier = 0.001f;
                Limb.PhysicalBehaviour.Properties.BulletSpeedAbsorptionPower = 0.5f;
                Limb.PhysicalBehaviour.TrueInitialMass /= 20;
                Limb.BreakingThreshold = Mathf.Infinity;

                usingBlade = false;
            }
        }

        IEnumerator PlaySound(AudioClip SoundToPlay)
        {
            var sound = new GameObject();
            sound.name = "SFX";
            //sound.transform.parent = transform;
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

    public class NanoHammer : Power, Mod.ModAPIPlus.IUse2
    {
        public bool usingBlade = false;
        public Sprite OGArmSprite;
        public Sprite Nanoblade = Mod.NanoHammer;
        public BoxCollider2D col;
        public LimbBehaviour Limb;
        public ParticleSystem effect;

        public static NanoHammer SetPower(PersonBehaviour person, LimbBehaviour limb, Sprite icon, Sprite Nanoshield = null)
        {
            var power = limb.gameObject.AddComponent<NanoHammer>();
            power.icon = icon;
            power.Name = "Nano Hammer";
            power.Description = "Creates a powerful hammer that can destroy the target!";
            power.targetLimb = Mod.ModAPIPlus.GetTargettedLimb(limb.gameObject);
            power.Nanoblade = Nanoshield ?? Mod.NanoHammer;
            power.Limb = limb;
            power.col = limb.gameObject.AddComponent<BoxCollider2D>();
            power.col.size = new Vector2(0.342398643f, 0.684489965f);
            power.col.offset = new Vector2(-0.0287246704f, -0.0861734152f);
            power.col.enabled = false;
            return power;
        }

        public override void DisablePower()
        {
            base.DisablePower();
            Unblade();
        }

        public override void Start()
        {
            base.Start();

            var muzzleflash = UnityEngine.Object.Instantiate(ModAPI.FindSpawnable("Blaster Rifle").Prefab.transform.GetChild(0).gameObject);
            muzzleflash.transform.parent = transform;
            muzzleflash.transform.localRotation = Quaternion.Euler(0, 0, 270);
            muzzleflash.transform.localPosition = Vector2.zero;

            if (transform.root.localScale.x < 0)
            {
                muzzleflash.transform.localRotation = Quaternion.Euler(0, 0, 90);
                muzzleflash.transform.localScale = new Vector2(0.8703f, 0.8703f);
            }
            effect = muzzleflash.GetComponent<ParticleSystem>();
        }

        public void Use2()
        {
            if (GetComponent<SpriteMergerAnimator>() || GetComponent<SpriteMergerAnimatorAdvanced>() || !Enabled)
                return;

            StartCoroutine(PlaySound(Mod.NanoGunTransform));

            if (usingBlade)
                Unblade();
            else
                Blade();
            

            foreach (var coll in Limb.Person.GetComponentsInChildren<Collider2D>())
            {
                Physics2D.IgnoreCollision(coll, col, true);
            }
        }

        public void Blade()
        {
            OGArmSprite = Functions.Clone(GetComponent<SpriteRenderer>().sprite);

            gameObject.AddComponent<SpriteMergerAnimatorAdvanced>().Initialize(gameObject.GetComponent<SpriteRenderer>().sprite, new List<Sprite> { Timtam.MergeSprites(OGArmSprite, Nanoblade), Timtam.MergeSprites(OGArmSprite, Nanoblade) }, gameObject.GetComponent<SpriteRenderer>().material, gameObject.GetComponent<SpriteRenderer>().material, 2, AnimationType.CircleOut, false, 1);

            foreach (var blast in GetComponents<BlasterBehaviour>())
            {
                Destroy(blast.Muzzleflash.gameObject);
                Destroy(blast.blasterSoundSource);
                Destroy(blast);
            }

            col.enabled = true;

            Limb.PhysicalBehaviour.BulletPenetration = false;
            Limb.ShotDamageMultiplier = 0;
            Limb.PhysicalBehaviour.Properties.BulletSpeedAbsorptionPower = 1;
            Limb.PhysicalBehaviour.TrueInitialMass *= 10;
            Limb.BreakingThreshold = Mathf.Infinity;
            usingBlade = true;
        }

        public void Unblade()
        {
            if (usingBlade)
            {
                gameObject.AddComponent<SpriteMergerAnimatorAdvanced>().Initialize(gameObject.GetComponent<SpriteRenderer>().sprite, new List<Sprite> { OGArmSprite, OGArmSprite }, gameObject.GetComponent<SpriteRenderer>().material, gameObject.GetComponent<SpriteRenderer>().material, 2, AnimationType.CircleIn, false, 1);

                col.enabled = false;

                Limb.PhysicalBehaviour.BulletPenetration = false;
                Limb.ShotDamageMultiplier = 0.001f;
                Limb.PhysicalBehaviour.Properties.BulletSpeedAbsorptionPower = 0.5f;
                Limb.PhysicalBehaviour.TrueInitialMass /= 10;
                Limb.BreakingThreshold = Mathf.Infinity;

                usingBlade = false;
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (!Enabled)
                return;

            Vector2 relativeVelocity = collision.relativeVelocity;

            float impactStrength = relativeVelocity.magnitude * 4;

            if (collision.gameObject.GetComponent<Rigidbody2D>() && collision.relativeVelocity.magnitude > 3)
            {
                Vector2 directionToThisObject = (GetComponent<Rigidbody2D>().position - collision.rigidbody.position).normalized;

                float dotProduct = Vector2.Dot(relativeVelocity, directionToThisObject);

                if (dotProduct > 0)
                {
                    collision.gameObject.GetComponent<Rigidbody2D>().velocity += -relativeVelocity.normalized * impactStrength;
                    CameraShakeBehaviour.main.Shake(7, base.transform.position);
                    if (collision.gameObject.GetComponent<PhysicalBehaviour>())
                        collision.gameObject.GetComponent<PhysicalBehaviour>().charge += impactStrength / 10;
                    ModAPI.CreateParticleEffect("HugeZap", base.transform.position);
                    var imp = ModAPI.FindSpawnable("Power Hammer").Prefab.GetComponent<PowerHammerBehaviour>().ImpactClips;
                    StartCoroutine(PlaySound(imp[UnityEngine.Random.Range(0, imp.Length)]));
                }

            }
        }

        IEnumerator PlaySound(AudioClip SoundToPlay)
        {
            var sound = new GameObject();
            sound.name = "SFX";
            //sound.transform.parent = transform;
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

    public class Nanotech : Power, Mod.ModAPIPlus.IUse2
    {
        public PersonBehaviour Person;

        public AudioClip TransformationSound = Mod.TransformationSound;

        bool TransformOnceNoAnim = false;
        bool hadHealing;
        bool hadStrength;

        public bool transformed = false;

        public List<Sprite> Sprites = new List<Sprite>();
        public List<Sprite> NanotechSuit = Mod.NanotechSuit;

        public Dictionary<LimbBehaviour, float> threshold = new Dictionary<LimbBehaviour, float>();

        public static Nanotech SetPower(PersonBehaviour Person, LimbBehaviour Limb, Sprite icon, List<Sprite> Nanotech = null)
        {
            var power = Limb.gameObject.AddComponent<Nanotech>();
            power.Name = "Nanotech";
            power.Description = "Nanotech suit that covers the user and protects them!\n<color=\"yellow\">Activate head with custom activation key (typically H) to transform the user";
            power.icon = icon;
            power.targetLimb = TargettedLimb.Head;
            power.Person = Limb.Person;
            power.NanotechSuit = Nanotech ?? Mod.NanotechSuit;

            ChestRepulsor.SetPower(Person, Person.Limbs[1], null);

            foreach (var limb in Person.Limbs)
            {
                power.threshold.Add(limb, limb.BreakingThreshold);

                limb.gameObject.GetOrAddComponent<NanoLimb>().tech = power;
                limb.BreakingThreshold = Mathf.Infinity;
                Sprite clonedSprite = Functions.Clone(limb.PhysicalBehaviour.spriteRenderer.sprite);
                clonedSprite.name = limb.name;
                power.Sprites.Add(clonedSprite);

                if (limb.name.Contains("LowerArm"))
                {
                    Repulsor.SetPower(Person, limb, null);
                    RepulsorCannons.SetPower(Person, limb, null);
                    NanoBlade.SetPower(Person, limb, null);
                    NanoShield.SetPower(Person, limb, null);
                    NanoHammer.SetPower(Person, limb, null);
                    limb.gameObject.AddComponent<AbilityCycler>().targetPowers = Mod.ModAPIPlus.GetTargettedLimb(limb.gameObject);
                }

                if (limb.name.Contains("Foot"))
                    Thruster.SetPower(Person, limb, null);

                try
                {
                    limb.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.First(a => a.Identity.Contains("Toggle nanotech on limb"));
                }
                catch
                {
                    limb.GetComponent<PhysicalBehaviour>().ContextMenuOptions.Buttons.Add(new ContextMenuButton("Toggle nanotech on limb", "Toggle nanotech on limb", "Toggle nanotech on limb", () =>
                    {

                        Sprite sprit = null;

                        if (limb.GetComponent<NanoLimb>().IsNano)
                        {
                            sprit = Timtam.GetLimbSprite(power.Sprites, limb);
                        }
                        else
                        {
                            sprit = Timtam.GetLimbSprite(power.NanotechSuit, limb);
                        }

                        List<Sprite> a = new List<Sprite> { sprit, sprit };
                        limb.GetComponent<NanoLimb>().IsNano = !limb.GetComponent<NanoLimb>().IsNano;
                        limb.gameObject.AddComponent<SpriteMergerAnimatorAdvanced>().Initialize(limb.PhysicalBehaviour.spriteRenderer.sprite, a, limb.PhysicalBehaviour.spriteRenderer.material, true, 1, AnimationType.CircleOut, true);
                    }));
                }
            }

            return power;
        }

        public void Use2()
        {
            if (transformed)
                TransformBack();
            else if (Enabled)
                Transform();
        }

        public void FixedUpdate()
        {
            if (TransformOnceNoAnim)
                if (!Person.GetComponentInChildren<SpriteMergerAnimatorAdvanced>())
                {
                    TransformOnceNoAnim = false;
                    Use2();
                }
        }

        public void Transform(int startlimb = 1)
        {
            if (Person.GetComponentInChildren<SpriteMergerAnimatorAdvanced>())
                return;

            foreach (var power in transform.root.GetComponentsInChildren<Power>())
            {
                if (power != this && power.Enabled)
                    power.DisablePower();
            }

            if (Person.GetComponentInChildren<SpriteMergerAnimatorAdvanced>() || !Enabled)
            {
                TransformOnceNoAnim = true;
                return;
            }

            transformed = true;

            foreach (var limb in Person.Limbs)
            {
                limb.PhysicalBehaviour.Properties = ModAPI.FindPhysicalProperties("AndroidArmour");
                limb.PhysicalBehaviour.BulletPenetration = false;
                limb.PhysicalBehaviour.Properties.BulletSpeedAbsorptionPower = 1;
                limb.ImpactDamageMultiplier = 0.001f;
                limb.ImpactPainMultiplier = 0;
                limb.ShotDamageMultiplier = 0;
                limb.BreakingThreshold = Mathf.Infinity;
                limb.ImmuneToDamage = true;
            }

            Sprites.Clear();
            foreach (var limb in Person.Limbs)
            {
                limb.GetComponent<NanoLimb>().IsNano = true;
                Sprite clonedSprite = Functions.Clone(limb.PhysicalBehaviour.spriteRenderer.sprite);
                clonedSprite.name = limb.name;
                Sprites.Add(clonedSprite);

                if (limb.GetComponent<LineRenderer>())
                {
                    limb.GetComponent<LineRenderer>().startColor = new Color(0, 0, 0, 0);
                    limb.GetComponent<LineRenderer>().endColor = new Color(0, 0, 0, 0);
                }
            }

            Timtam.MakeCustomSkinSpread(Person.Limbs[startlimb], NanotechSuit, false, true, 2, true, Mod.Nanounder, 1);

            Person.Limbs[1].GetComponent<ChestRepulsor>().EnablePower();

            foreach (var limb in Person.Limbs)
            {
                if (limb.name.Contains("LowerArm"))
                {
                    limb.GetComponent<Repulsor>().EnablePower();
                }
                else if (limb.name.Contains("Foot"))
                {
                    limb.GetComponent<Thruster>().EnablePower();
                }

                if (Person.TryGetComponent<SpeedHealing>(out var heal))
                {
                    if (heal.Enabled)
                        hadHealing = true;
                    else
                    {
                        hadHealing = false;
                        heal.EnablePower();
                    }
                }
                else
                {
                    hadHealing = false;
                    SpeedHealing.SetPower(Person, null).EnablePower();
                }

                if (Person.TryGetComponent<SuperMass>(out var mass))
                {
                    if (mass.Enabled)
                        hadStrength = true;
                    else
                    {
                        hadStrength = false;
                        mass.EnablePower();
                    }
                }
                else
                {
                    hadStrength = false;
                    SuperMass.SetPower(Person, null).EnablePower();
                }

                Person.Limbs[2].PhysicalBehaviour.PlayClipOnce(TransformationSound);
            }
        }

        public void TransformBack()
        {
            if (Person.GetComponentInChildren<SpriteMergerAnimatorAdvanced>())
                return;

            foreach (var power in transform.root.GetComponentsInChildren<Power>())
            {
                if (power != this && power.Enabled)
                    power.DisablePower();
            }

            if (Person.GetComponentInChildren<SpriteMergerAnimatorAdvanced>())
            {
                TransformOnceNoAnim = true;
                return;
            }

            transformed = false;

            foreach (var limb in Person.Limbs)
            {
                limb.PhysicalBehaviour.Properties = ModAPI.FindPhysicalProperties("Human");
                limb.PhysicalBehaviour.BulletPenetration = true;
                limb.ImpactDamageMultiplier = 1;
                limb.ImpactPainMultiplier = 1;
                limb.ShotDamageMultiplier = 1;
                limb.ImmuneToDamage = false;
            }

            Person.Limbs[2].PhysicalBehaviour.PlayClipOnce(TransformationSound);

            foreach (var limb in GetDeepestPushedToLimbs(Person.Limbs[1]))
            {
                limb.GetComponent<NanoLimb>().IsNano = false;
                Timtam.MakeCustomSkinSpread(limb, Sprites, false, true, 1, true, Sprites, 1, true);

                if (limb.TryGetComponent<LineRenderer>(out var line))
                {
                    line.startColor = Color.white;
                    line.endColor = Color.white;
                }

            }

            if (!hadHealing)
                Person.GetComponent<SpeedHealing>().DisablePower();

            if (!hadStrength)
                Person.GetComponent<SuperMass>().DisablePower();
        }

        public static List<LimbBehaviour> GetDeepestPushedToLimbs(LimbBehaviour rootLimb)
        {
            var result = new List<LimbBehaviour>();
            var visited = new HashSet<CirculationBehaviour>();

            void Traverse(CirculationBehaviour cb)
            {
                if (cb == null || visited.Contains(cb))
                    return;

                visited.Add(cb);

                if (cb.PushesTo == null || cb.PushesTo.Count() == 0)
                {
                    if (cb.Limb != null)
                        result.Add(cb.Limb);
                    return;
                }

                foreach (var nextCb in cb.PushesTo)
                {
                    Traverse(nextCb);
                }
            }

            if (rootLimb?.CirculationBehaviour != null)
                Traverse(rootLimb.CirculationBehaviour);

            return result;
        }

        [SkipSerialisation]
        public class NanoLimb : MonoBehaviour
        {
            public Nanotech tech;
            public bool IsNano = false;
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
                    CameraShakeBehaviour.main.Shake(6, transform.position);

                    ModAPI.CreateParticleEffect("Vapor", transform.position).transform.localScale = Vector3.one * 3;
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


    //----------------------
    // Hello! if you are thinking about using any of my code, feel free to contact me on discord (_timtams.) using any of my code without my permission will get your mod taken down :P
    //----------------------

    public class AbilityCycler : MonoBehaviour, Mod.ModAPIPlus.ISwap
    {
        public TargettedLimb targetPowers;
        public List<Power> powers = new List<Power>();

        public void Swap()
        {
            foreach (var power in transform.root.GetComponentsInChildren<Power>())
            {
                if (power.targetLimb == targetPowers)
                    if (!powers.Contains(power))
                        powers.Add(power);
            }

            try
            {
                powers.First(a => a.Name == "No Power");
            }
            catch
            {
                var noPower = transform.root.GetComponentsInChildren<Power>().FirstOrDefault(a => a.Name == "No Power");
                if (noPower != null)
                {
                    powers.Add(noPower);
                }
            }

            if (powers.Count > 0)
            {
                int currentIndex = powers.FindIndex(p => p.Enabled);

                foreach (var power in powers)
                {
                    power.DisablePower();
                }

                if (currentIndex != -1)
                {
                    int nextIndex = (currentIndex + 1) % powers.Count;
                    powers[nextIndex].EnablePower();
                    FakeNotifDissapearer.CreateNotification(powers[nextIndex].Name, transform.position);

                    Debug.Log(powers[nextIndex].Name);
                }
                else
                {
                    powers[0].EnablePower();
                    Debug.Log(powers[0].Name);
                    FakeNotifDissapearer.CreateNotification(powers[0].Name, transform.position);
                }
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

            if (InputSystem.Down("NovaSwapActivation"))
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
                                if (comp is Mod.ModAPIPlus.ISwap us)
                                {
                                    us.Swap();
                                }
                            }
                        }
                    }

                    foreach (var comp in underMouse.GetComponents<MonoBehaviour>())
                    {
                        if (comp is Mod.ModAPIPlus.ISwap us &&
                            !(underMouse.TryGetComponent<LimbBehaviour>(out var lb) && lb.name.Contains("LowerLeg")))
                        {
                            us.Swap();
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
                                if (comp is Mod.ModAPIPlus.ISwap us)
                                {
                                    us.Swap();
                                }
                            }
                        }
                    }

                    foreach (var comp in hit.GetComponents<MonoBehaviour>())
                    {
                        if (comp is Mod.ModAPIPlus.ISwap us &&
                            !(hit.TryGetComponent<LimbBehaviour>(out var lb) && lb.name.Contains("LowerLeg")) && hit != SelectionController.Main.CurrentlyUnderMouse)
                        {
                            us.Swap();
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
            power.Description = "Activate the user's Head to allow them to teleport to the location of your mouse cursor when left click is pressed";
            power.icon = icon;

            power.targetLimb = TargettedLimb.Head;
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
            OGDistance = springJoint ? springJoint.distance : 0;
            OGStrength = springJoint ? springJoint.frequency : 1;
        }

        public void Update()
        {
            if (!springJoint)
                return;

            float currentDistance = Vector2.Distance(transform.position, springJoint.connectedAnchor);

            if (springJoint.connectedBody)
                currentDistance = Vector2.Distance(transform.position, springJoint.connectedBody.transform.TransformPoint(springJoint.connectedAnchor));

            if (currentDistance < OGDistance)
            {
                springJoint.frequency = 0.0001f;
            }
            else
                springJoint.frequency = OGStrength;
        }
    }

    public class FakeNotifDissapearer : MonoBehaviour
    {
        private float fadeSpeed = 0.8f;
        private float lastAlpha = 1f;
        public static GameObject Start;

        public static void CreateNotification(string text, Vector2 pos)
        {
            var notif = Instantiate(Start);
            notif.GetComponentInChildren<TextMeshProUGUI>().text = text;
            notif.AddComponent<FakeNotifDissapearer>();
            notif.transform.position = pos;
        }

        public void Update()
        {
            var sr = GetComponentInChildren<TextMeshProUGUI>();
            var sa = GetComponent<SpriteRenderer>();

            Color targetColor = new Color(1, 1, 1, 0);

            if (sr)
            {
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
            else
            {
                if (sa.color.a > 0.01f)
                {
                    // Use unscaledDeltaTime to ignore timescale
                    sa.color = Color.Lerp(sa.color, targetColor, fadeSpeed * Time.unscaledDeltaTime * 10f);
                    lastAlpha = sa.color.a;
                }
                else
                {
                    Destroy(gameObject);
                }
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
        public Color color;

        public void Start()
        {
            if (TryGetComponent<SpriteRenderer>(out var sr))
                sr.color = color;
        }

        void JointCreated(SpringJoint2D joint)
        {
            var energyWire = joint.gameObject.AddComponent<WebWireBehaviour>();
            energyWire.WireColor = color;
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
                    sr.color = new Color(color.r, color.g, color.b, 0.4f);
                    var breakweb = collision.gameObject.AddComponent<BreakWebDecal>();
                    breakweb.webdecal = webDecal;
                    breakweb.joint = grappleJoint;
                    Destroy(gameObject);
                }
                else
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
                    sr.color = new Color(color.r, color.g, color.b, 0.4f);
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
        public Color webColor = Color.white;
        WebType webType = WebType.Normal;
        RaycastHit2D hit;

        public void Use2()
        {
            if (!Enabled)
                return;

            int nextType = ((int)webType + 1) % Enum.GetValues(typeof(WebType)).Length;
            webType = (WebType)nextType;
            if (Settings.main.Get<bool>("SyncedWeb"))

                foreach (var web in transform.root.GetComponent<PersonBehaviour>().GetComponentsInChildren<WebSlinging>())
                    if (web != this)
                        web.webType = webType;

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

        public static WebSlinging SetPower(PersonBehaviour Person, LimbBehaviour Limb, Sprite icon)
        {
            var power = Limb.gameObject.AddComponent<WebSlinging>();
            power.Name = "Webslinging";
            power.Description = "Allows the user to fire webs with the activation key.<color=\"yellow\">\nRight click to toggle web rigidity\nUse alternate activation key (typically H) to toggle web type\n Web Types: Normal, Connector, Grapple, Electric, Webshot, None";
            power.icon = icon;
            int webTypeIndex = Settings.main.Get<int>("DefaultWeb");
            power.webType = webTypeIndex == 2 ? WebType.Connector :
                            webTypeIndex == 3 ? WebType.Grapple :
                            webTypeIndex == 4 ? WebType.Electric :
                            webTypeIndex == 5 ? WebType.Webshot :
                            webTypeIndex == 6 ? WebType.None : WebType.Normal;

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

            if (GetComponent<Grabber>())
                GetComponent<Grabber>().enabled = true;

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

        public override void EnablePower()
        {
            base.EnablePower();
            if (GetComponent<Grabber>())
                GetComponent<Grabber>().enabled = false;
        }

        public void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void JointCreated(SpringJoint2D joint)
        {
            var energyWire = joint.gameObject.AddComponent<WebWireBehaviour>();
            energyWire.WireColor = webColor;
            energyWire.WireMaterial = Instantiate(ModAPI.FindSpawnable("Brick").Prefab.GetComponent<SpriteRenderer>().material);
            energyWire.WireWidth = 0.055f;
            energyWire.typedJoint = joint;
            energyWire.WireMaterial.mainTexture = webType == WebType.Electric ? Mod.electricWeb.texture : Mod.web.texture;
            energyWire.WireMaterial.mainTexture.wrapMode = TextureWrapMode.Repeat;
        }

        void JointCreated(DistanceJoint2D joint)
        {
            var energyWire = joint.gameObject.AddComponent<EnergyWireBehaviour>();
            energyWire.WireColor = webColor;
            energyWire.WireMaterial = Instantiate(ModAPI.FindSpawnable("Brick").Prefab.GetComponent<SpriteRenderer>().material);
            energyWire.WireWidth = 0.055f;
            energyWire.typedJoint = joint;
            energyWire.WireMaterial.mainTexture = webType == WebType.Electric ? Mod.electricWeb.texture : Mod.web.texture;
            energyWire.WireMaterial.mainTexture.wrapMode = TextureWrapMode.Repeat;
        }

        public void Use(ActivationPropagation activation)
        {
            Rigid = Settings.main.Get<bool>("RigidWebs");
            float weblength = Settings.main.Get<float>("WebLength");
            float webshotforce = Settings.main.Get<float>("WebshotForce");
            float webstrength = Settings.main.Get<int>("RigidWebStrength");

            if (webType == WebType.Webshot && Enabled)
            {
                var webshot = ModAPI.CreatePhysicalObject("Webshot", Mod.WebShot);
                webshot.GetComponent<PhysicalBehaviour>().Properties = ModAPI.FindPhysicalProperties("Soap");
                webshot.transform.position = transform.position + transform.up * -0.5f;
                webshot.GetComponent<Rigidbody2D>().velocity = transform.up * -webshotforce;
                webshot.GetComponent<Rigidbody2D>().angularVelocity = UnityEngine.Random.Range(-25, 25);
                webshot.AddComponent<WebShot>();
                webshot.GetComponent<WebShot>().person = GetComponent<LimbBehaviour>().Person;
                webshot.GetComponent<WebShot>().color = webColor;
                Debug.Log(GetComponent<LimbBehaviour>().Person.name);
                webshot.AddComponent<TrailRenderer>().startColor = webColor;
                webshot.GetComponent<TrailRenderer>().endColor = webColor;
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
                    float maxDistance = weblength;

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
                            hit = hitt; break;
                        }
                    }

                    if (hits == null)
                    {
                        ishit = false;
                    }

                    if (hit.collider != null && hit.rigidbody != null && !hit.collider.name.Contains("Point") && ishit == true)
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
                            newrigidjoint.breakForce = webstrength * hit.rigidbody.mass;
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
            else if (grappleJoint == null && rigidGrappleJoint == null && Enabled)
            {
                if (webType == WebType.None || webType == WebType.Webshot)
                    return;
                Vector2 origin = transform.position + transform.up * -0.1f;
                Vector2 direction = -transform.up;
                float maxDistance = weblength;

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
                        if (hitt.collider.GetComponent<PhysicalBehaviour>())
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
                        if (limb.Person == GetComponent<LimbBehaviour>().Person)
                            ishit = false;
                    }
                }

                if (hit.collider != null && hit.rigidbody != null && ishit == true)
                {
                    if (hit.collider.TryGetComponent<LimbBehaviour>(out var lim))
                        if (lim.Person == GetComponent<LimbBehaviour>().Person)
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
                        rigidGrappleJoint.breakForce = webstrength * hit.rigidbody.mass;
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
                        rigidGrappleJoint.connectedAnchor = origin + direction * (maxDistance / 2);
                        rigidGrappleJoint.autoConfigureDistance = false;
                        rigidGrappleJoint.enableCollision = true;
                        rigidGrappleJoint.distance = Vector2.Distance(transform.position, origin + direction * (maxDistance / 2));
                        rigidGrappleJoint.maxDistanceOnly = true;
                        rigidGrappleJoint.breakForce = webstrength * 0.75f;
                        OGDistance = rigidGrappleJoint.distance;
                        JointCreated(rigidGrappleJoint);
                    }
                    else
                    {
                        grappleJoint = gameObject.AddComponent<SpringJoint2D>();
                        grappleJoint.anchor = new Vector2(0, -0.25f);
                        grappleJoint.connectedAnchor = origin + direction * (maxDistance / 2);
                        grappleJoint.autoConfigureDistance = false;
                        grappleJoint.dampingRatio = 0.9f;
                        grappleJoint.frequency = 4f;
                        grappleJoint.enableCollision = true;
                        grappleJoint.distance = Vector2.Distance(transform.position, origin + direction * (maxDistance / 2));
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

                if (grappleJoint.connectedBody)
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

            if (rigidGrappleJoint && webType == WebType.Electric && grappleJoint.connectedAnchor != null)
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

    public class ShieldModeSwitch : MonoBehaviour, Messages.IOnDrop, Messages.IOnGripped, Messages.IUse
    {
        public Sprite SideView;
        public Sprite FrontView;

        GripBehaviour gripBehaviour;

        public void Use(ActivationPropagation activation)
        {
            if (GetComponent<SpriteRenderer>().sprite == SideView)
            {
                GetComponent<SpriteRenderer>().sprite = FrontView;
                GetComponent<PhysicalBehaviour>().RefreshOutline();
                Destroy(GetComponent<Collider2D>());
                gameObject.AddComponent<CircleCollider2D>();
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = SideView;
                GetComponent<PhysicalBehaviour>().RefreshOutline();
                Destroy(GetComponent<Collider2D>());
                gameObject.AddComponent<PolygonCollider2D>();
            }
        }

        public void OnDrop(GripBehaviour gripper)
        {
            gripBehaviour = null;
        }

        public void OnGripped(GripBehaviour gripper)
        {
            gripBehaviour = gripper;
        }

        public void FixedUpdate()
        {
            if (gripBehaviour != null)
            {
                foreach (var col in GetComponents<Collider2D>())
                {
                    foreach (var limb in gripBehaviour.transform.root.gameObject.GetComponent<PersonBehaviour>().Limbs)
                    {
                        if (limb.GetComponent<Collider2D>())
                        {
                            Physics2D.IgnoreCollision(col, limb.GetComponent<Collider2D>(), true);
                        }
                    }
                }
            }

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
                        if (CameraShakeBehaviour.main != null)
                        {
                            CameraShakeBehaviour.main.Shake(col.relativeVelocity.magnitude * 0.1f, col.contacts[0].point, 1);
                        }
                        ModAPI.CreateParticleEffect("Vapor", col.contacts[0].point);
                    }

                    if (col.collider != null && col.collider.gameObject != null && col.collider.gameObject.TryGetComponent<LimbBehaviour>(out var limb))
                    {
                        limb.Damage(col.relativeVelocity.magnitude * strength);
                        if (col.relativeVelocity.magnitude > 10 / strength)
                        {
                            var effect = ModAPI.CreateParticleEffect("BloodExplosion", col.contacts[0].point);
                            var ps = effect != null ? effect.GetComponent<ParticleSystem>() : null;
                            if (ps != null && limb.CirculationBehaviour != null)
                            {
                                ps.startColor = limb.CirculationBehaviour.GetComputedColor();
                            }
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
            foreach (var limb in Personb.Limbs)
            {
                limb.ImpactDamageMultiplier /= 5f;
                limb.ImpactPainMultiplier /= 5f;
            }
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
            foreach (var limb in Personb.Limbs)
            {
                limb.ImpactDamageMultiplier /= 15f;
                limb.ImpactPainMultiplier /= 15f;
            }
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
            int damdamp = Settings.main.Get<int>("DamDamp");

            foreach (var limb in Person.Limbs)
            {
                limb.ImpactDamageMultiplier /= damdamp * newMass;
                limb.ImpactPainMultiplier /= damdamp * newMass;
            }
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

    public class DynamicCape : MonoBehaviour
    {
        public int numberOfPoints = 20;
        public float pointSpacing = 0.085f;
        public Vector3 offset = new Vector3(-0.13f, 0.12f, 0);
        public LineRenderer lineRenderer;
        public GameObject[] capePoints;
        private Vector3[] positions;
        public Texture2D capeTexture;
        public Sprite CapeCollar;
        public GameObject CapeCollarOBJ;

        public static void CreateCapeForPerson(PersonBehaviour person, LimbBehaviour limb, Texture2D capeTex, Vector3 attachOffset)
        {
            float capeLength = capeTex.width;
            int dynamicPoints = capeTex.width / 2;

            var cape = limb.gameObject.AddComponent<DynamicCape>();
            cape.capeTexture = capeTex;
            cape.numberOfPoints = dynamicPoints;
            cape.pointSpacing = capeLength / (dynamicPoints - 1) * ModAPI.PixelSize;
            cape.offset = attachOffset;

            cape.CapeCollarOBJ = new GameObject("CapeCollar");
            cape.CapeCollarOBJ.transform.parent = limb.transform;
            cape.CapeCollarOBJ.transform.localPosition = Vector3.zero;
            cape.CapeCollarOBJ.transform.localRotation = Quaternion.identity;
            cape.CapeCollarOBJ.transform.localScale = Vector3.one;
            var sr = cape.CapeCollarOBJ.AddComponent<SpriteRenderer>();
            sr.sprite = cape.CapeCollar;
            sr.sortingLayerID = limb.GetComponent<SpriteRenderer>().sortingLayerID;
            sr.sortingOrder = limb.GetComponent<SpriteRenderer>().sortingOrder + 1;

            cape.lineRenderer = cape.gameObject.AddComponent<LineRenderer>();
            cape.lineRenderer.positionCount = dynamicPoints;
            cape.lineRenderer.startWidth = capeTex.height * ModAPI.PixelSize;
            cape.lineRenderer.endWidth = capeTex.height * ModAPI.PixelSize;
            Material material = new Material(Shader.Find("Sprites/Default"))
            {
                mainTexture = capeTex
            };
            cape.lineRenderer.material = material;
            cape.lineRenderer.startColor = Color.white;
            cape.lineRenderer.endColor = Color.white;
            cape.lineRenderer.useWorldSpace = false;
            cape.lineRenderer.alignment = LineAlignment.View;
            cape.lineRenderer.sortingLayerName = limb.GetComponent<SpriteRenderer>().sortingLayerName;
            cape.lineRenderer.sortingOrder = limb.GetComponent<SpriteRenderer>().sortingOrder + 5;

            cape.capePoints = new GameObject[dynamicPoints];
            cape.positions = new Vector3[dynamicPoints];
            for (int i = 0; i < dynamicPoints; i++)
            {
                GameObject point = new GameObject("CapePoint" + i);
                point.transform.parent = cape.transform;
                if (i == 0)
                    point.transform.localPosition = cape.offset;
                else
                    point.transform.localPosition = cape.offset + new Vector3(0, -i * cape.pointSpacing, 0);

                Rigidbody2D rb = point.AddComponent<Rigidbody2D>();
                rb.gravityScale = 1f;
                rb.mass = 0.001f;
                rb.drag = 1.2f;
                CircleCollider2D collider = point.AddComponent<CircleCollider2D>();
                collider.radius = 0.05f;

                cape.capePoints[i] = point;
                cape.positions[i] = point.transform.localPosition;
            }

            FixedJoint2D firstJoint = cape.capePoints[0].AddComponent<FixedJoint2D>();
            firstJoint.connectedBody = cape.GetComponent<Rigidbody2D>();

            for (int i = 0; i < dynamicPoints - 1; i++)
            {
                DistanceJoint2D joint = cape.capePoints[i].AddComponent<DistanceJoint2D>();
                joint.connectedBody = cape.capePoints[i + 1].GetComponent<Rigidbody2D>();
                cape.capePoints[i + 1].GetComponent<Rigidbody2D>().interpolation = RigidbodyInterpolation2D.Interpolate;
                joint.autoConfigureDistance = false;
                joint.distance = cape.pointSpacing;
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

    public class Cape : MonoBehaviour
    {
        public int numberOfPoints = 20;
        public float pointSpacing = 0.085f;
        public Vector3 offset = new Vector3(-0.13f, 0.12f, 0);
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
                Material material = new Material(Shader.Find("Sprites/Default"))
                {
                    mainTexture = cape.capeTexture
                };
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

    public enum AnimationType
    {
        Random,
        CircleOut,
        CircleIn,
        TopToBottom,
        BottomToTop,
        LeftToRight,
        RightToLeft,
        HorizontalMiddle,
        VerticalMiddle
    }

    // This class and its functions are completely off limits for use outside of my (Timtams, _timtams. on discord) mods, I will not give it out.
    // If it is found in other mods, it will be taken down with a formal DMCA request.
    public class SpriteMergerAnimatorAdvanced : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        private Texture2D baseTexture;
        private List<Texture2D> overlayTextures;
        private List<List<Vector2Int>> orderedPixelsLayers;
        private Material material;

        private Texture2D centeredFleshTex;
        private Texture2D centeredBoneTex;
        private Texture2D centeredDamageTex;

        private Rect finalRect;
        private bool removeExcessTransparent;
        private bool randomizeSpread;
        private AnimationType animationType;
        private float layerSpreadDuration;
        private int layerCount;

        public UnityEvent OnAnimationComplete = new UnityEvent();

        private Color bruiseColor;
        private Color secondBruiseColor;
        private Color thirdBruiseColor;
        private Color zombie;
        private Color glowColour;
        private Color freezeColour;
        private Color electroBoneColour;
        private Color bloodColor;

        private List<List<Vector2Int>> GroupPixelsByLayer(List<Vector2Int> pixels, AnimationType type, int width, int height)
        {
            var layers = new List<List<Vector2Int>>();
            Vector2 center = new Vector2(width / 2f, height / 2f);

            foreach (var pixel in pixels)
            {
                int layerIdx = 0;
                switch (type)
                {
                    case AnimationType.CircleOut:
                        layerIdx = Mathf.RoundToInt(Vector2.Distance(center, pixel));
                        break;
                    case AnimationType.CircleIn:
                        layerIdx = Mathf.RoundToInt(Vector2.Distance(center, pixel));
                        layerIdx = (int)center.x + (int)center.y - layerIdx;
                        break;
                    case AnimationType.TopToBottom:
                        layerIdx = pixel.y;
                        break;
                    case AnimationType.BottomToTop:
                        layerIdx = height - pixel.y - 1;
                        break;
                    case AnimationType.LeftToRight:
                        layerIdx = pixel.x;
                        break;
                    case AnimationType.RightToLeft:
                        layerIdx = width - pixel.x - 1;
                        break;
                    case AnimationType.VerticalMiddle:
                        layerIdx = Mathf.RoundToInt(Mathf.Abs(pixel.x - center.x));
                        break;
                    case AnimationType.HorizontalMiddle:
                        layerIdx = Mathf.RoundToInt(Mathf.Abs(pixel.y - center.y));
                        break;
                    case AnimationType.Random:
                    default:
                        layerIdx = 0;
                        break;
                }
                while (layers.Count <= layerIdx)
                    layers.Add(new List<Vector2Int>());
                layers[layerIdx].Add(pixel);
            }
            return layers;
        }

        private List<List<Vector2Int>> GetPixelsOrderedLayers(Texture2D overlay, Texture2D baseTex, AnimationType type, bool randomize)
        {
            int width = overlay.width;
            int height = overlay.height;
            var pixels = new List<Vector2Int>();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color overlayPixel = overlay.GetPixel(x, y);
                    Color basePixel = baseTex.GetPixel(x, y);
                    if (overlayPixel.a > 0 || basePixel.a > 0)
                    {
                        pixels.Add(new Vector2Int(x, y));
                    }
                }
            }

            var layers = GroupPixelsByLayer(pixels, type, width, height);

            if (randomize)
            {
                System.Random rng = new System.Random();
                for (int i = 0; i < layers.Count; i++)
                {
                    layers[i] = layers[i].OrderBy(p => rng.Next()).ToList();
                }
            }
            return layers;
        }

        public void Initialize(
            Sprite baseSprite,
            List<Sprite> overlaySprites,
            Material material,
            bool removeExcessTransparent = true,
            float durationSeconds = 2f,
            AnimationType animationType = AnimationType.TopToBottom,
            bool randomizeSpread = true,
            int pixelLayersPerOverlay = 1
        )
        {
            overlaySprites.Add(overlaySprites[overlaySprites.Count - 1]);
            overlaySprites.Add(overlaySprites[overlaySprites.Count - 1]);
            overlaySprites.Add(overlaySprites[overlaySprites.Count - 1]);
            overlaySprites.Add(overlaySprites[overlaySprites.Count - 1]);
            overlaySprites.Add(overlaySprites[overlaySprites.Count - 1]);

            spriteRenderer = GetComponent<SpriteRenderer>();
            this.material = material;
            this.removeExcessTransparent = removeExcessTransparent;
            this.randomizeSpread = randomizeSpread;
            this.animationType = animationType;
            this.layerCount = overlaySprites.Count;

            // Save shader properties
            bruiseColor = material.GetColor("_BruiseColor");
            secondBruiseColor = material.GetColor("_SecondBruiseColor");
            thirdBruiseColor = material.GetColor("_ThirdBruiseColor");
            zombie = material.GetColor("_Zombie");
            glowColour = material.GetColor("_GlowColour");
            freezeColour = material.GetColor("_FreezeColour");
            electroBoneColour = material.GetColor("_ElectroBoneColour");
            bloodColor = material.GetColor("_BloodColor");

            int finalWidth = (int)baseSprite.rect.width;
            int finalHeight = (int)baseSprite.rect.height;
            foreach (var spr in overlaySprites)
            {
                finalWidth = Mathf.Max(finalWidth, (int)spr.rect.width);
                finalHeight = Mathf.Max(finalHeight, (int)spr.rect.height);
            }
            finalRect = new Rect(0, 0, finalWidth, finalHeight);

            baseTexture = CreateTextureFromSprite(baseSprite, finalRect);

            overlayTextures = new List<Texture2D>();
            foreach (var spr in overlaySprites)
            {
                overlayTextures.Add(CreateTextureFromSprite(spr, finalRect));
            }

            centeredFleshTex = CreateCenteredTexture(material.GetTexture("_FleshTex") as Texture2D, baseSprite.rect, finalRect);
            centeredBoneTex = CreateCenteredTexture(material.GetTexture("_BoneTex") as Texture2D, baseSprite.rect, finalRect);
            centeredDamageTex = CreateCenteredDamageTexture(Mod.DamageTexture, finalRect);

            List<List<List<Vector2Int>>> overlaysLayers = new List<List<List<Vector2Int>>>();
            foreach (var overlayTex in overlayTextures)
            {
                overlaysLayers.Add(GroupPixelsByLayer(
                    GetPixelsOrderedLayers(overlayTex, baseTexture, animationType, randomizeSpread)
                        .SelectMany(l => l).ToList(),
                    animationType, overlayTex.width, overlayTex.height));
            }

            StartCoroutine(MergeSpritesSpreadOverTimeLayers(
                durationSeconds, overlaysLayers, overlayTextures, baseTexture, finalRect, pixelLayersPerOverlay));
        }

        public void Initialize(
            Sprite baseSprite,
            Sprite overlaySprite,
            Material material,
            bool removeExcessTransparent = true,
            float durationSeconds = 8f,
            AnimationType animationType = AnimationType.TopToBottom,
            bool randomizeSpread = true)
        {
            Initialize(baseSprite, new List<Sprite> { overlaySprite }, material, removeExcessTransparent, durationSeconds, animationType, randomizeSpread, 0);
        }

        private IEnumerator MergeSpritesSpreadOverTimeLayers(
            float totalDuration,
            List<List<List<Vector2Int>>> overlaysLayers,
            List<Texture2D> overlays,
            Texture2D baseTexture,
            Rect finalRect,
            int pixelLayersBehind,
            bool randomizeAcrossLayers = true
        )
        {
            int overlaysCount = overlays.Count;
            var rng = new System.Random();

            int totalPixels = overlaysLayers.Sum(ol => ol.Sum(layer => layer.Count));
            float frames = totalDuration / Time.fixedDeltaTime;
            int pixelsPerFrame = Mathf.CeilToInt(totalPixels / frames);

            int[][] consumed = new int[overlaysCount][];
            for (int o = 0; o < overlaysCount; o++)
                consumed[o] = new int[overlaysLayers[o].Count];

            bool firstLayerCompletedInvoked = false;
            bool firstLayerJustCompleted = false;

            HashSet<Vector2Int>[] erasedPixels = new HashSet<Vector2Int>[overlaysCount];
            for (int o = 0; o < overlaysCount; o++)
                erasedPixels[o] = new HashSet<Vector2Int>();

            while (true)
            {
                bool allDone = true;
                int budget = pixelsPerFrame;

                for (int o = 0; o < overlaysCount && budget > 0; o++)
                {
                    var layers = overlaysLayers[o];
                    var tex = overlays[o];

                    int first = 0;
                    while (first < layers.Count && consumed[o][first] >= layers[first].Count)
                        first++;

                    if (first >= layers.Count)
                    {
                        if (o == 0 && !firstLayerCompletedInvoked)
                        {
                            firstLayerCompletedInvoked = true;
                            firstLayerJustCompleted = true;
                            OnAnimationComplete?.Invoke();
                        }
                        continue;
                    }

                    if (o > 0)
                    {
                        int prevFirst = 0;
                        var prevLayers = overlaysLayers[o - 1];
                        while (prevFirst < prevLayers.Count &&
                               consumed[o - 1][prevFirst] >= prevLayers[prevFirst].Count)
                            prevFirst++;

                        if (first > prevFirst + pixelLayersBehind)
                            continue;
                    }

                    allDone = false;

                    var candidates = new List<int>();
                    var weights = new List<float>();
                    for (int L = first; L < layers.Count; L++)
                    {
                        int remain = layers[L].Count - consumed[o][L];
                        if (remain <= 0) continue;
                        candidates.Add(L);

                        if (!randomizeAcrossLayers)
                        {
                            weights.Add(1f);
                            break;
                        }
                        else
                        {
                            float dist = L - first;
                            float exponent = 4f;
                            weights.Add(1f / Mathf.Pow(dist + 1f, exponent));
                        }
                    }

                    if (candidates.Count == 0)
                        continue;

                    float totalW = weights.Sum();

                    while (budget > 0 && candidates.Count > 0)
                    {
                        float pick = (float)rng.NextDouble() * totalW;
                        float accum = 0f;
                        int chosen = candidates[0];
                        int wi = 0;
                        for (; wi < candidates.Count; wi++)
                        {
                            accum += weights[wi];
                            if (pick <= accum)
                            {
                                chosen = candidates[wi];
                                break;
                            }
                        }

                        var pList = layers[chosen];
                        int idx = consumed[o][chosen];
                        var pos = pList[idx];

                        Color cO = tex.GetPixel(pos.x, pos.y);

                        baseTexture.SetPixel(pos.x, pos.y, cO);

                        UpdateShaderTextures(pos);

                        consumed[o][chosen]++;
                        budget--;

                        if (consumed[o][chosen] >= pList.Count)
                        {
                            totalW -= weights[wi];
                            candidates.RemoveAt(wi);
                            weights.RemoveAt(wi);
                        }
                    }
                }

                
                baseTexture.Apply();
                spriteRenderer.sprite = Sprite.Create(
                    baseTexture, finalRect, Vector2.one * 0.5f,
                    spriteRenderer.sprite.pixelsPerUnit
                );
                if (firstLayerJustCompleted)
                {
                    firstLayerJustCompleted = false;
                    if (IsFinalAppearanceReached(overlays[overlays.Count - 1], baseTexture))
                    {
                        break;
                    }
                }

                if (allDone) break;
                yield return new WaitForFixedUpdate();
            }

            OnAnimationComplete?.Invoke();

            // Restore shader colours
            SetShaderColour(material, "_BruiseColor", bruiseColor);
            SetShaderColour(material, "_SecondBruiseColor", secondBruiseColor);
            SetShaderColour(material, "_ThirdBruiseColor", thirdBruiseColor);
            SetShaderColour(material, "_Zombie", zombie);
            SetShaderColour(material, "_GlowColour", glowColour);
            SetShaderColour(material, "_FreezeColour", freezeColour);
            SetShaderColour(material, "_ElectroBoneColour", electroBoneColour);
            SetShaderColour(material, "_BloodColor", bloodColor);

            GetComponent<PhysicalBehaviour>().RefreshOutline();
            Destroy(this);
        }

        private bool IsFinalAppearanceReached(Texture2D finalTex, Texture2D currentTex)
        {
            if (!finalTex || !currentTex) return false;
            if (finalTex.width != currentTex.width || finalTex.height != currentTex.height) return false;

            var a = currentTex.GetPixels32();
            var b = finalTex.GetPixels32();
            if (a.Length != b.Length) return false;

            for (int i = 0; i < a.Length; i++)
            {
                if (a[i].r != b[i].r || a[i].g != b[i].g || a[i].b != b[i].b || a[i].a != b[i].a)
                    return false;
            }
            return true;
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
                for (int x = 0; x < width; x++)
                    texture.SetPixel(x, y, clearColor);

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
                for (int x = 0; x < width; x++)
                    newTexture.SetPixel(x, y, clearColor);

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
                for (int x = 0; x < width; x++)
                    newTexture.SetPixel(x, y, clearColor);

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

        private void SetShaderColour(Material mat, string id, Color color)
        {
            int nameID = ShaderProperties.Get(id);
            mat.SetColor(nameID, color);
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

    public class SliderValueDisplay : MonoBehaviour
    {
        public TextMeshProUGUI Displayer;
        public Slider value;

        public void Initialize(Slider slider, TextMeshProUGUI display)
        {
            value = slider;
            Displayer = display;
        }

        private string FormatValue(float val)
        {
            string formatted = Math.Round(val, 3).ToString("0.###");
            int decimalIndex = formatted.IndexOf('.');
            if (decimalIndex >= 0)
            {
                int decimals = formatted.Length - decimalIndex - 1;
                if (decimals == 2)
                    formatted += "0";
            }
            return formatted;
        }

        void Update()
        {
            // Replace the line in SliderValueDisplay.Update()
            Displayer.text = "Value: " + FormatValue(value.value);
        }
    }

    public class UIScaler : MonoBehaviour
    {
        public float scaleFactor = 0.8f; // How much smaller than the screen the UI should be

        private Canvas canvas;
        private RectTransform rectTransform;

        void Awake()
        {
            canvas = GameObject.FindObjectOfType<Canvas>();
            rectTransform = GetComponent<RectTransform>();
        }

        public void OnEnable()
        {
            if (PlayerPrefs.GetInt("Nova_Shown_UI_Dialog") == 0)
            {
                DialogBoxManager.Notification("Change this mod's UI scale in the settings menu at the top of the category \n(this message only appears once)");
                PlayerPrefs.SetInt("Nova_Shown_UI_Dialog", 1);
            }
        }

        private void Update()
        {
            if (rectTransform == null || canvas == null)
                return;

            try
            {
                scaleFactor = Settings.main.Get<float>("UISize");
            }
            catch
            {

            }

            Vector2 screenSize = canvas.renderMode == RenderMode.ScreenSpaceOverlay || canvas.renderMode == RenderMode.ScreenSpaceCamera
                ? new Vector2(Screen.width, Screen.height)
                : rectTransform.rect.size;

            Vector2 uiSize = rectTransform.rect.size;

            float widthRatio = (screenSize.x * scaleFactor) / uiSize.x;
            float heightRatio = (screenSize.y * scaleFactor) / uiSize.y;
            float minRatio = Mathf.Min(widthRatio, heightRatio, 10f);

            transform.localScale = Vector3.one * minRatio;
        }
    }

    public static class Timtam
    {
        // less laggy varient
        public static void CreateFastCollider(SpriteRenderer sr, float alphaThreshold = 0.1f)
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
            List<Vector2> points = new List<Vector2>();
            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                    if (pixels[y * w + x].a >= alphaThreshold)
                        points.Add(new Vector2(x, y));

            if (points.Count < 3)
                return;

            points.Sort((a, b) => a.x == b.x ? a.y.CompareTo(b.y) : a.x.CompareTo(b.x));
            List<Vector2> hull = new List<Vector2>();
            for (int i = 0; i < points.Count; i++)
            {
                while (hull.Count >= 2 && Cross(hull[hull.Count - 2], hull[hull.Count - 1], points[i]) <= 0)
                    hull.RemoveAt(hull.Count - 1);
                hull.Add(points[i]);
            }
            int t = hull.Count + 1;
            for (int i = points.Count - 2; i >= 0; i--)
            {
                while (hull.Count >= t && Cross(hull[hull.Count - 2], hull[hull.Count - 1], points[i]) <= 0)
                    hull.RemoveAt(hull.Count - 1);
                hull.Add(points[i]);
            }
            hull.RemoveAt(hull.Count - 1);

            float ppu = sprite.pixelsPerUnit;
            Vector2 pivot = sprite.pivot;
            Vector2[] path = new Vector2[hull.Count];
            for (int i = 0; i < hull.Count; i++)
            {
                Vector2 fullSpacePx = hull[i] + tOffset;
                path[i] = (fullSpacePx - pivot) / ppu;
            }
            collider.pathCount = 1;
            collider.SetPath(0, path);

            float Cross(Vector2 o, Vector2 a, Vector2 b)
            {
                return (a.x - o.x) * (b.y - o.y) - (a.y - o.y) * (b.x - o.x);
            }
        }

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
                    Color baseColor = mergedTexture.GetPixel(overlayXOffset + x, overlayYOffset + y);

                    if (overlayColor.a > 0)
                    {
                        if (baseColor.a > 0 || !preserveBaseTransparency)
                        {
                            float finalAlpha = overlayColor.a + baseColor.a * (1 - overlayColor.a);
                            Color blendedColor = (overlayColor * overlayColor.a + baseColor * baseColor.a * (1 - overlayColor.a)) / finalAlpha;
                            blendedColor.a = finalAlpha;
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
                    Color baseColor = mergedTexture.GetPixel(overlayXOffset + x, overlayYOffset + y);

                    if (overlayColor.a > 0)
                    {
                        if (baseColor.a > 0 || !preserveBaseTransparency)
                        {
                            float finalAlpha = overlayColor.a + baseColor.a * (1 - overlayColor.a);
                            Color blendedColor = (overlayColor * overlayColor.a + baseColor * baseColor.a * (1 - overlayColor.a)) / finalAlpha;
                            blendedColor.a = finalAlpha;
                            mergedTexture.SetPixel(overlayXOffset + x, overlayYOffset + y, blendedColor);
                        }
                    }
                    else
                    {
                        mergedTexture.SetPixel(overlayXOffset + x, overlayYOffset + y, clearColor);
                    }
                }
            }

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

        public static void MakeCustomSkin(PersonBehaviour person, List<Sprite> sprites, bool WrapOverBasically = false, bool OverlayOnly = false)
        {
            foreach (var limb in person.Limbs)
            {
                var sprite = GetLimbSprite(sprites, limb);

                if (sprite == null)
                {
                    continue;
                }

                var Head = limb.GetComponent<SpriteRenderer>();
                Head.sprite = Timtam.MergeSpritesWithShaderHandling(Head.sprite, sprite, Head.material, WrapOverBasically, OverlayOnly);
                Head.GetComponent<PhysicalBehaviour>().RefreshOutline();
            }
        }

        public static void MakeCustomSkinAnimated(PersonBehaviour person, List<Sprite> sprites, bool WrapOverBasically = false, bool OverlayOnly = false, float duration = 2, AnimationType animtype = AnimationType.CircleOut, bool random = true, List<Sprite> other = null)
        {
            foreach (var limb in person.Limbs)
            {
                if (limb.GetComponent<SpriteMergerAnimatorAdvanced>())
                    continue;

                Sprite sprite = GetLimbSprite(sprites, limb);
                var othersprite = GetLimbSprite(other, limb);

                var sr = limb.GetComponent<SpriteRenderer>();
                if (other != null)
                    limb.gameObject.AddComponent<SpriteMergerAnimatorAdvanced>().Initialize(sr.sprite, new List<Sprite> { othersprite, sprite }, sr.material, true, duration, animtype, random);
                else
                    limb.gameObject.AddComponent<SpriteMergerAnimatorAdvanced>().Initialize(sr.sprite, new List<Sprite> { sprite, sprite }, sr.material, true, duration, animtype, random);
            }
            person.SetBruiseColor(130, 10, 10);             // Purple
            person.SetSecondBruiseColor(180, 20, 20);        // Indigo (deep purple)
            person.SetThirdBruiseColor(139, 10, 10);         // Dark magenta (reddish purple)
            person.SetRottenColour(139, 0, 10);         // Medium orchid (purple tint)
            person.SetBloodColour(139, 0, 10);            // Dark red
        }

        public static void MakeCustomSkinSpread(LimbBehaviour limb, List<Sprite> sprites, bool WrapOverBasically = false, bool OverlayOnly = false, float speed = 4, bool randomize = true, List<Sprite> layer2 = null, int otherspeed = 3, bool invertcircle = false)
        {
            Sprite sprite = GetLimbSprite(sprites, limb);
            Debug.Log(sprite.name);
            var sr = limb.GetComponent<SpriteRenderer>();
            var merg = limb.gameObject.AddComponent<SpriteMergerAnimatorAdvanced>();

            merg.OnAnimationComplete.AddListener(() =>
            {
                SpreadSprite(limb.CirculationBehaviour, limb, sprites, layer2, speed, randomize, otherspeed);
            });

            if (layer2 != null)
            {
                Sprite sprite2 = GetLimbSprite(layer2, limb);
                List<Sprite> spritess = new List<Sprite> { sprite2, sprite, sprite };
                merg.Initialize(sr.sprite, spritess, sr.material, true, speed, invertcircle ? AnimationType.CircleIn : AnimationType.CircleOut, randomize);
            }
            else
            {
                merg.Initialize(sr.sprite, sprite, sr.material, true, speed, invertcircle ? AnimationType.CircleIn : AnimationType.CircleOut, randomize);
            }
        }

        public static Sprite GetLimbSprite(List<Sprite> sprites, LimbBehaviour limb)
        {
            if (sprites == null || limb == null || string.IsNullOrEmpty(limb.name))
                return null;

            Sprite matchedd = sprites.FirstOrDefault(s => s != null && s.name == limb.name);

            if (matchedd == null)
            {
                string replacedName = limb.name.Replace("Front", "");
                matchedd = sprites.FirstOrDefault(s => s != null && s.name == replacedName);
            }

            return matchedd;
        }
        public static void SpreadSprite(CirculationBehaviour source, LimbBehaviour limb, List<Sprite> sprites1, List<Sprite> sprites2 = null, float speed = 4, bool randomized = true, int otherspeed = 3, HashSet<CirculationBehaviour> processedLimbs = null)
        {
            if (processedLimbs == null)
                processedLimbs = new HashSet<CirculationBehaviour>();

            processedLimbs.Add(limb.CirculationBehaviour);

            foreach (var connectedLimb in limb.CirculationBehaviour.PushesTo)
            {
                if (processedLimbs.Contains(connectedLimb))
                    continue;

                Sprite matched = GetLimbSprite(sprites1, connectedLimb.Limb);

                if (!matched)
                    continue;

                if (!connectedLimb.Limb.GetComponent<SpriteMergerAnimatorAdvanced>() && connectedLimb != source)
                {
                    processedLimbs.Add(connectedLimb);

                    var sr = connectedLimb.Limb.GetComponent<SpriteRenderer>();
                    var merg = connectedLimb.Limb.gameObject.AddComponent<SpriteMergerAnimatorAdvanced>();

                    merg.OnAnimationComplete.AddListener(() =>
                    {
                        SpreadSprite(limb.CirculationBehaviour, connectedLimb.Limb, sprites1, sprites2, speed, randomized, otherspeed, processedLimbs);
                    });

                    if (sprites2 != null)
                    {
                        Sprite sprite2 = GetLimbSprite(sprites2, connectedLimb.Limb);
                        List<Sprite> spritess = new List<Sprite> { sprite2, matched, matched };
                        merg.Initialize(sr.sprite, spritess, sr.material, true, speed, connectedLimb.name == "Head" ? AnimationType.TopToBottom : AnimationType.BottomToTop, randomized);
                    }
                    else
                    {
                        merg.Initialize(sr.sprite, matched, sr.material, true, speed, AnimationType.BottomToTop, randomized);
                    }
                }
            }

            if (limb.CirculationBehaviour.Source != null && limb.CirculationBehaviour.Source != source)
            {
                if (processedLimbs.Contains(limb.CirculationBehaviour.Source))
                    return;

                var connectedLimb = limb.CirculationBehaviour.Source;
                Sprite matched = GetLimbSprite(sprites1, connectedLimb.Limb);

                if (!matched)
                    return;

                if (!connectedLimb.Limb.GetComponent<SpriteMergerAnimatorAdvanced>())
                {
                    processedLimbs.Add(connectedLimb);

                    var sr = connectedLimb.Limb.GetComponent<SpriteRenderer>();
                    var merg = connectedLimb.Limb.gameObject.AddComponent<SpriteMergerAnimatorAdvanced>();

                    merg.OnAnimationComplete.AddListener(() =>
                    {
                        SpreadSprite(limb.CirculationBehaviour, connectedLimb.Limb, sprites1, sprites2, speed, randomized, otherspeed, processedLimbs);
                    });

                    if (sprites2 != null)
                    {
                        Sprite sprite2 = GetLimbSprite(sprites2, connectedLimb.Limb);
                        List<Sprite> spritess = new List<Sprite> { sprite2, matched, matched };
                        merg.Initialize(sr.sprite, spritess, sr.material, true, speed, AnimationType.TopToBottom, randomized);
                    }
                    else
                    {
                        merg.Initialize(sr.sprite, matched, sr.material, true, speed, AnimationType.TopToBottom, randomized);
                    }
                }
            }
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
            power.Name = "No Power";
            power.targetLimb = Mod.ModAPIPlus.GetTargettedLimb(TargetLimb.gameObject);
            return power;
        }

        public static Power SetPower(PersonBehaviour Person)
        {
            var power = Person.gameObject.AddComponent<NoPower>();
            power.Name = "No Power";
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

            if (targetLimb != TargettedLimb.Internal)
            {
                foreach (Power power2 in transform.root.GetComponentsInChildren<Power>())
                {
                    if (power2 != this && power2.targetLimb == targetLimb && power2.Enabled)
                        power2.DisablePower();
                }
            }
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
                if (contextMenu.activeInHierarchy)
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
            ui.gameObject.AddComponent<UIScaler>();
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

        public void OnDestroy()
        {
            if (ui != null)
            {
                Destroy(ui);
            }
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

            if (!transform.root.GetComponent<AbilityMenus>())
                transform.root.gameObject.AddComponent<AbilityMenus>();

            for (int i = 0; i < Buttons.Count; i++)
            {
                Buttons[i].Item1.transform.SetParent(ui.transform.FindChild("Scroll View").FindChild("Viewport").FindChild("Content"));
                Buttons[i].Item1.transform.localScale = Vector3.one;
            }
        }

        public void OnEnable()
        {
            UpdateButtons();
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
            ui.gameObject.AddComponent<UIScaler>();
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

        void UpdateButtons()
        {
            foreach (var button in Buttons)
            {
                if (button.Item2.Enabled)
                {
                    button.Item1.transform.GetChild(0).gameObject.SetActive(false);
                }
                else
                {
                    button.Item1.transform.GetChild(0).gameObject.SetActive(true);
                }
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
            ui.gameObject.AddComponent<UIScaler>();
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
                power.button = button;
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
                GameObject offThing = new GameObject("OffThing");
                offThing.transform.parent = buttonTransform.transform;
                var offRect = offThing.AddComponent<RectTransform>();
                offRect.anchorMin = Vector2.zero;
                offRect.anchorMax = Vector2.one;
                offRect.offsetMin = Vector2.zero;
                offRect.offsetMax = Vector2.zero;
                offThing.AddComponent<Image>().sprite = InternalPowerMenu.ToggledSprite;
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
            if (!contextMenu)
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
            ui.gameObject.AddComponent<UIScaler>();
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

        public void AddFakeButton(string name, Sprite sprite, List<Sprite> limbs, UnityEvent OnAddEvent = null, UnityEvent OnRemoveEvent = null, Sprite Cape = null, Sprite CapeCollar = null)
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
                    SelectedEvent = OnAddEvent;
                    DeselectedEvent = OnRemoveEvent;
                    Selnametext.text = name;

                    foreach (var ci in PreviewHuman.GetComponentsInChildren<Image>())
                    {
                        ci.sprite = null;
                        ci.color = new Color(0, 0, 0, 0);

                        foreach (var limb in limbs)
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

    public class SettingsMenu : MonoBehaviour
    {
        public GameObject ui;
        public Transform Contents;
        public static GameObject MenuPrefab;
        public static GameObject FloatSetting;
        public static GameObject IntSetting;
        public static GameObject BoolSetting;
        public static GameObject StringSetting;

        public void Start()
        {
            if (!Settings.SettingsMenuCurrentUI)
                CreateUI();
            else
                Destroy(gameObject);
        }

        public void CreateUI()
        {
            GameObject canvas = GameObject.Find("Canvas");
            ui = Instantiate(MenuPrefab);
            Settings.SettingsMenuCurrentUI = ui;
            ui.gameObject.AddComponent<UIScaler>();
            ui.transform.SetParent(canvas.transform);
            ui.transform.SetSiblingIndex(1);
            ui.transform.localScale = Vector3.one * 1.5f;
            ui.GetComponent<RectTransform>().sizeDelta = new Vector2(755.4285f, 782.1189f);
            ui.GetComponent<RectTransform>().localPosition = Vector2.zero;
            Contents = ui.transform.FindChild("Scroll View").GetChild(0).GetChild(0);

            GameObject button = ui.transform.FindChild("Button").gameObject;

            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                Destroy(ui);
                Settings.SettingsMenuCurrentUI = null;
                Destroy(this.gameObject);
            });

            PopulateSettings();
        }

        public void PopulateSettings()
        {
            foreach (var setting in Settings.main.settings)
            {
                if (setting.Value.ValueType == typeof(float))
                {
                    InstantiateSetting(setting.Value.Min != setting.Value.Max ? FloatSetting : StringSetting, setting.Key, setting.Value);
                }
                else if (setting.Value.ValueType == typeof(int))
                {
                    InstantiateSetting(setting.Value.Min != setting.Value.Max ? FloatSetting : StringSetting, setting.Key, setting.Value);
                }
                else if (setting.Value.ValueType == typeof(bool))
                {
                    InstantiateSetting(BoolSetting, setting.Key, setting.Value);
                }
                else if (setting.Value.ValueType == typeof(string))
                {
                    InstantiateSetting(StringSetting, setting.Key, setting.Value);
                }
            }

            foreach (var button in ui.GetComponentsInChildren<Button>())
            {
                button.onClick.AddListener(() =>
                {
                    UISoundBehaviour.Main.Blip();
                });
            }

            foreach (var input in Contents.GetComponentsInChildren<TMP_InputField>(true))
            {
                input.onSelect.AddListener((text) =>
                {
                    Global.main.AddUiBlocker();
                });

                input.onDeselect.AddListener((text) =>
                {
                    Global.main.RemoveUiBlocker();
                });
            }
        }

        private void InstantiateSetting(GameObject prefab, string key, Settings.Setting setting)
        {
            GameObject settingObject = Instantiate(prefab, Contents);
            settingObject.name = setting.Name;
            settingObject.transform.localScale = Vector3.one;

            TextMeshProUGUI label = settingObject.transform.FindChild("NameDisplay").GetChild(0).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI description = settingObject.transform.FindChild("Description").GetComponent<TextMeshProUGUI>();

            label.text = setting.Name;
            description.text = setting.Description;

            TextMeshProUGUI value = settingObject.transform.FindChild("Value")?.GetComponent<TextMeshProUGUI>();
            if (setting.ValueType == typeof(float))
            {
                Slider slider = settingObject.GetComponentInChildren<Slider>();
                TMP_InputField inputField = settingObject.GetComponentInChildren<TMP_InputField>();
                if (slider != null)
                {
                    Debug.Log(Settings.main.Get<float>(key));
                    slider.minValue = setting.Min;
                    slider.maxValue = setting.Max;
                    slider.interactable = true;
                    slider.value = Settings.main.Get<float>(key);

                    value.gameObject.AddComponent<SliderValueDisplay>().Initialize(slider, value);
                    UnityAction<float> callback = (UnityAction<float>)Delegate.CreateDelegate(typeof(UnityAction<float>), UISoundBehaviour.Main, typeof(UISoundBehaviour).GetMethod("SliderCallback", BindingFlags.Instance | BindingFlags.NonPublic));
                    slider.onValueChanged.AddListener(callback);

                    slider.onValueChanged.AddListener(newValue =>
                    {
                        Settings.main.Set<float>(key, newValue);
                    });
                }
                else
                {
                    var placeholder = inputField.placeholder as TextMeshProUGUI;
                    placeholder.text = "Original Value: " + setting.DefaultValue.ToString();
                    inputField.contentType = TMP_InputField.ContentType.DecimalNumber;
                    inputField.text = Settings.main.Get<float>(key).ToString();

                    inputField.onValueChanged.AddListener(newValue =>
                    {
                        if (float.TryParse(newValue, out float parsedValue))
                        {
                            Settings.main.Set<float>(key, parsedValue);
                        }
                    });
                }
            }
            else if (setting.ValueType == typeof(int) && setting.ValueType != typeof(float))
            {
                Slider slider = settingObject.GetComponentInChildren<Slider>();
                TMP_InputField inputField = settingObject.GetComponentInChildren<TMP_InputField>();
                if (slider != null)
                {

                    slider.minValue = setting.Min;
                    slider.maxValue = setting.Max;
                    slider.interactable = true;
                    slider.wholeNumbers = true;
                    slider.value = Settings.main.Get<int>(key);

                    value.gameObject.AddComponent<SliderValueDisplay>().Initialize(slider, value);
                    UnityAction<float> callback = (UnityAction<float>)Delegate.CreateDelegate(typeof(UnityAction<float>), UISoundBehaviour.Main, typeof(UISoundBehaviour).GetMethod("SliderCallback", BindingFlags.Instance | BindingFlags.NonPublic));
                    slider.onValueChanged.AddListener(callback);

                    slider.onValueChanged.AddListener(newValue =>
                    {
                        Settings.main.Set<int>(key, Mathf.RoundToInt(newValue));
                    });
                }
                else
                {
                    var placeholder = inputField.placeholder as TextMeshProUGUI;
                    placeholder.text = "Original Value: " + setting.DefaultValue.ToString();
                    inputField.contentType = TMP_InputField.ContentType.IntegerNumber;
                    inputField.text = Settings.main.Get<int>(key).ToString();

                    inputField.onValueChanged.AddListener(newValue =>
                    {
                        if (int.TryParse(newValue, out int parsedValue))
                        {
                            Settings.main.Set<int>(key, parsedValue);
                        }
                    });
                }
            }
            else if (setting.ValueType == typeof(bool))
            {
                GameObject toggle = settingObject.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
                var button = settingObject.GetComponentInChildren<Button>();

                toggle.SetActive(Settings.main.Get<bool>(key));

                button.onClick.AddListener(() =>
                {
                    bool currentValue2 = Settings.main.Get<bool>(key);
                    bool newValue = !currentValue2;
                    Settings.main.Set<bool>(key, newValue);
                    toggle.SetActive(newValue);
                });
            }
            else if (setting.ValueType == typeof(string) && setting.ValueType != typeof(int) && setting.ValueType != typeof(float))
            {
                TMP_InputField inputField = settingObject.GetComponentInChildren<TMP_InputField>();
                if (inputField != null)
                {
                    var placeholder = inputField.placeholder as TextMeshProUGUI;
                    placeholder.text = setting.DefaultValue.ToString();
                    inputField.contentType = TMP_InputField.ContentType.Standard;
                    inputField.text = Settings.main.Get<string>(key);

                    inputField.onValueChanged.AddListener(newValue =>
                    {
                        Settings.main.Set<string>(key, newValue);
                    });
                }
            }
        }
    }

    public class Settings
    {
        public static Settings main;

        public static GameObject SettingsMenuCurrentUI;

        public Dictionary<string, Setting> settings = new Dictionary<string, Setting>();

        public void AddSetting(string name, string description, string playerPrefKey, object defaultValue, Type valueType, float minval = 0, float maxval = 0)
        {
            settings[playerPrefKey] = new Setting(name, description, playerPrefKey, defaultValue, valueType, minval, maxval);
        }

        public T Get<T>(string key)
        {
            if (settings.TryGetValue(key, out Setting setting))
            {
                return (T)setting.GetValue();
            }
            throw new KeyNotFoundException($"Setting with key '{key}' not found.");
        }

        public void Set<T>(string key, T value)
        {
            if (settings.TryGetValue(key, out Setting setting))
            {
                setting.SetValue(value);
            }
            else
            {
                throw new KeyNotFoundException($"Setting with key '{key}' not found.");
            }
        }

        public void LoadAll()
        {
            foreach (var setting in settings.Values)
            {
                setting.GetValue();
            }
        }

        public void ResetAll()
        {
            foreach (var setting in settings.Values)
            {
                setting.SetValue(setting.DefaultValue);
            }
        }

        public class Setting
        {
            public string Name;
            public string Description;
            public string PlayerPrefKey;
            public object DefaultValue;
            public float Max;
            public float Min;
            public Type ValueType;

            public Setting(string name, string description, string playerPrefKey, object defaultValue, Type valueType, float minval = 0, float maxval = 1)
            {
                Name = name;
                Description = description;
                PlayerPrefKey = playerPrefKey;
                DefaultValue = defaultValue;
                ValueType = valueType;
                Min = minval;
                Max = maxval;
            }

            public object GetValue()
            {
                if (ValueType == typeof(bool))
                    return PlayerPrefs.GetInt(PlayerPrefKey, Convert.ToInt32(DefaultValue)) == 1;
                else if (ValueType == typeof(int))
                    return PlayerPrefs.GetInt(PlayerPrefKey, Convert.ToInt32(DefaultValue));
                else if (ValueType == typeof(float))
                    return PlayerPrefs.GetFloat(PlayerPrefKey, Convert.ToSingle(DefaultValue));
                else if (ValueType == typeof(string))
                    return PlayerPrefs.GetString(PlayerPrefKey, Convert.ToString(DefaultValue));
                return DefaultValue;
            }

            public bool GetBoolValue()
            {
                return (bool)GetValue();
            }

            public void SetValue(object value)
            {
                if (ValueType == typeof(bool))
                    PlayerPrefs.SetInt(PlayerPrefKey, (bool)value ? 1 : 0);
                else if (ValueType == typeof(int))
                    PlayerPrefs.SetInt(PlayerPrefKey, (int)value);
                else if (ValueType == typeof(float))
                    PlayerPrefs.SetFloat(PlayerPrefKey, (float)value);
                else if (ValueType == typeof(string))
                    PlayerPrefs.SetString(PlayerPrefKey, (string)value);
                PlayerPrefs.Save();
            }
        }
    }

    public static class Functions
    {
        public static void CreatePicker<T>(UnityAction<T> PickedFunction) where T : Component
        {
            GameObject Picker = new GameObject("Picker");
            Picker PickerComponent = Picker.AddComponent<Picker>();
            PickerComponent.Upds(PickedFunction);
        }

        public static Sprite CreateSprite(Rect rect, Texture2D tex)
        {
            return Sprite.Create(tex, new Rect(rect.position, rect.size), Vector2.one * .5f, 35f);
        }

        public static Sprite CreateSprite(Texture2D tex)
        {
            return Sprite.Create(tex, new Rect(0, 0, (int)tex.width, (int)tex.height), Vector2.one * .5f, 35f);
        }

        public static Texture2D Clone(Texture2D Tex)
        {
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
        public static Texture2D Clone(Texture2D Tex, Sprite spr)
        {
            RenderTexture rt = RenderTexture.GetTemporary(Tex.width, Tex.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.sRGB);
            Graphics.Blit(Tex, rt);
            RenderTexture prev = RenderTexture.active;
            RenderTexture.active = rt;
            Texture2D clone = new Texture2D((int)spr.textureRect.width, (int)spr.textureRect.height, TextureFormat.RGBA32, false) { filterMode = FilterMode.Point };
            clone.ReadPixels(spr.textureRect, 0, 0);
            clone.Apply();
            RenderTexture.active = prev;
            RenderTexture.ReleaseTemporary(rt);
            return clone;
        }
        public static Texture2D CropToContent(Texture2D tex)
        {
            int width = tex.width;
            int height = tex.height;

            int xMin = width, xMax = 0, yMin = height, yMax = 0;
            bool found = false;

            // Find bounds of non-transparent content
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (tex.GetPixel(x, y).a > 0f)
                    {
                        if (x < xMin) xMin = x;
                        if (x > xMax) xMax = x;
                        if (y < yMin) yMin = y;
                        if (y > yMax) yMax = y;
                        found = true;
                    }
                }
            }

            if (!found)
                return tex; // No content, return original

            int contentWidth = xMax - xMin + 1;
            int contentHeight = yMax - yMin + 1;

            // Maintain original aspect ratio by padding
            float origRatio = (float)width / height;
            float contentRatio = (float)contentWidth / contentHeight;

            int padLeft = 0, padRight = 0, padTop = 0, padBottom = 0;

            if (contentRatio > origRatio)
            {
                // Need to pad height
                int targetHeight = Mathf.RoundToInt(contentWidth / origRatio);
                int pad = targetHeight - contentHeight;
                padTop = pad / 2;
                padBottom = pad - padTop;
            }
            else if (contentRatio < origRatio)
            {
                // Need to pad width
                int targetWidth = Mathf.RoundToInt(contentHeight * origRatio);
                int pad = targetWidth - contentWidth;
                padLeft = pad / 2;
                padRight = pad - padLeft;
            }

            int newWidth = contentWidth + padLeft + padRight;
            int newHeight = contentHeight + padTop + padBottom;

            Texture2D newTex = new Texture2D(newWidth, newHeight, tex.format, false);
            newTex.filterMode = tex.filterMode;
            newTex.anisoLevel = tex.anisoLevel;

            // Fill transparent
            Color clear = new Color(0, 0, 0, 0);
            for (int y = 0; y < newHeight; y++)
                for (int x = 0; x < newWidth; x++)
                    newTex.SetPixel(x, y, clear);

            // Copy content
            for (int y = 0; y < contentHeight; y++)
            {
                for (int x = 0; x < contentWidth; x++)
                {
                    Color c = tex.GetPixel(xMin + x, yMin + y);
                    newTex.SetPixel(x + padLeft, y + padTop, c);
                }
            }
            newTex.Apply();

            return newTex;
        }
        public static Sprite Clone(Sprite spr)
        {
            Sprite s = Sprite.Create(Clone(spr.texture), new Rect(spr.textureRect.position - spr.textureRectOffset, spr.rect.size), Vector2.one * .5f, spr.pixelsPerUnit, 0U, SpriteMeshType.FullRect, spr.border, false);
            s.name = spr.name;
            return s;
        }
        public static Sprite Clone(Sprite spr, Texture2D tex)
        {
            return Sprite.Create(tex, new Rect(spr.textureRect.position - spr.textureRectOffset, spr.rect.size), Vector2.one * .5f, spr.pixelsPerUnit, 0U, SpriteMeshType.FullRect, spr.border, false);
        }
        public static int GetArea(Texture2D Tex, Rect rect = default)
        {
            int res = 0;
            if (rect != default)
            {
                foreach (Color i in Tex.GetPixels((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height))
                {
                    if (i.a > 0) ++res;
                }
            }
            else
            {
                foreach (Color i in Tex.GetPixels())
                {
                    if (i.a > 0) ++res;
                }
            }
            return res;
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

    //ModUtils class created by RikuTheKiller on discord
    public static class ModUtils
    {
        public static void RealignLimb(LimbBehaviour limb)
        {
            PersonBehaviour person = limb.GetComponentInParent<PersonBehaviour>();

            if (GetDescendantLimbs(limb) != null && person && limb)
            {
                foreach (LimbBehaviour i in GetDescendantLimbs(limb))
                {
                    if (i.TryGetComponent(out Mod.LimbLoggerBehaviour iLogger) && iLogger.ConnectedBody.TryGetComponent(out PhysicalBehaviour iConnectedPhysical))
                    {
                        if (!iConnectedPhysical.isDisintegrated)
                        {
                            i.transform.position = iLogger.ConnectedBody.transform.TransformPoint(iLogger.Offset);
                            i.transform.rotation = iLogger.ConnectedBody.transform.rotation;
                        }
                    }
                }
                foreach (LimbBehaviour i in person.Limbs)
                {
                    if (i.TryGetComponent(out Rigidbody2D rigidBody))
                    {
                        rigidBody.rotation = i.transform.eulerAngles.z;
                    }
                }
            }
        }

        public static Vector3 DivideVector3(Vector3 a, Vector3 b)
        {
            return new Vector3(!float.IsNaN(a.x / b.x) ? a.x / b.x : 0f, !float.IsNaN(a.y / b.y) ? a.y / b.y : 0f, !float.IsNaN(a.z / b.z) ? a.z / b.z : 0f);
        }

        public static LimbBehaviour[] GetDescendantLimbs(LimbBehaviour limb)
        {
            PersonBehaviour person = limb.GetComponentInParent<PersonBehaviour>();
            List<LimbBehaviour> connectedLimbs = new List<LimbBehaviour>();
            List<LimbBehaviour> limbsToAdd = new List<LimbBehaviour>();
            int previousCount = 0;

            if (person && limb)
            {
                connectedLimbs.Add(limb);
                while (connectedLimbs.Count > previousCount)
                {
                    previousCount = connectedLimbs.Count;
                    foreach (LimbBehaviour i in person.Limbs)
                    {
                        if (i.TryGetComponent(out Mod.LimbLoggerBehaviour iLogger) && !i.IsDismembered)
                        {
                            foreach (LimbBehaviour i2 in connectedLimbs)
                            {
                                if (i2.name == iLogger.ConnectedBody.name)
                                {
                                    if (!connectedLimbs.Contains(i))
                                    {
                                        limbsToAdd.Add(i);
                                    }
                                }
                            }
                        }
                    }
                    foreach (LimbBehaviour i in limbsToAdd)
                    {
                        connectedLimbs.Add(i);
                    }
                    limbsToAdd.Clear();
                }
                return connectedLimbs.ToArray();
            }
            return null;
        }

        public static LimbBehaviour WhatToConnectTo(LimbBehaviour limb, bool bypassBackupCheck = false)
        {
            PersonBehaviour person = limb.GetComponentInParent<PersonBehaviour>();
            LimbBehaviour upperBody = null;
            if (limb.name == "UpperBody" || limb.GetComponent<PhysicalBehaviour>().isDisintegrated)
            {
                return null;
            }
            foreach (LimbBehaviour i in person.Limbs)
            {
                if (i.name == "UpperBody" && !i.GetComponent<PhysicalBehaviour>().isDisintegrated)
                {
                    upperBody = i;
                }
            }
            if (upperBody)
            {
                foreach (LimbBehaviour i in person.Limbs)
                {
                    if (limb == i || i.GetComponent<PhysicalBehaviour>().isDisintegrated)
                    {
                        continue;
                    }
                    if ((limb.name == "Head" || limb.name == "MiddleBody" || limb.name == "UpperArmFront" || limb.name == "UpperArm") && i == upperBody)
                    {
                        return i;
                    }
                    if (limb.name == "LowerBody" && i.name == "MiddleBody")
                    {
                        return i;
                    }
                    if ((limb.name == "UpperLegFront" || limb.name == "UpperLeg") && i.name == "LowerBody")
                    {
                        return i;
                    }
                    if (limb.name == "LowerLegFront" && i.name == "UpperLegFront")
                    {
                        return i;
                    }
                    if (limb.name == "LowerLeg" && i.name == "UpperLeg")
                    {
                        return i;
                    }
                    if (limb.name == "FootFront" && i.name == "LowerLegFront")
                    {
                        return i;
                    }
                    if (limb.name == "Foot" && i.name == "LowerLeg")
                    {
                        return i;
                    }
                    if (limb.name == "LowerArmFront" && i.name == "UpperArmFront")
                    {
                        return i;
                    }
                    if (limb.name == "LowerArm" && i.name == "UpperArm")
                    {
                        return i;
                    }
                }
                if (!bypassBackupCheck)
                {
                    return upperBody;
                }
            }
            return null;
        }

        public static bool IsConnectionGone(LimbBehaviour limb)
        {
            PersonBehaviour person = limb.GetComponentInParent<PersonBehaviour>();
            if (limb.GetComponent<PhysicalBehaviour>().isDisintegrated)
            {
                return false;
            }
            foreach (LimbBehaviour i in person.Limbs)
            {
                if (limb == i)
                {
                    continue;
                }
                if ((limb.name == "Head" || limb.name == "MiddleBody" || limb.name == "UpperArmFront" || limb.name == "UpperArm") && i.name == "UpperBody")
                {
                    return false;
                }
                if (limb.name == "LowerBody" && i.name == "MiddleBody")
                {
                    return false;
                }
                if ((limb.name == "UpperLegFront" || limb.name == "UpperLeg") && i.name == "LowerBody")
                {
                    return false;
                }
                if (limb.name == "LowerLegFront" && i.name == "UpperLegFront")
                {
                    return false;
                }
                if (limb.name == "LowerLeg" && i.name == "UpperLeg")
                {
                    return false;
                }
                if (limb.name == "FootFront" && i.name == "LowerLegFront")
                {
                    return false;
                }
                if (limb.name == "Foot" && i.name == "LowerLeg")
                {
                    return false;
                }
                if (limb.name == "LowerArmFront" && i.name == "UpperArmFront")
                {
                    return false;
                }
                if (limb.name == "LowerArm" && i.name == "UpperArm")
                {
                    return false;
                }
            }
            return true;
        }
    }
}
//this mod was created by Nova Interactive, Don't take our code without permission or we'll kill your family https://www.patreon.com/c/NovaInt
