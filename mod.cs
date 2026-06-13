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
using static Mod.InfinityGauntlet;
using static Mod.Layers;

#pragma warning disable CS0618

/*
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

        #region backendshit

        #region loadSprites

        public static Texture2D DamageTexture = ModAPI.LoadSprite("Art/TempForScripts/DamageTexture.png").texture;

        public static Sprite NanotechIcon = ModAPI.LoadSprite("Art/UI/Icons/MindLaser.png");
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
        public static Sprite Bubble = ModAPI.LoadSprite("Art/Objects/Bubble.png");
        public static Sprite ToggledSprite = ModAPI.LoadSprite("Art/UI/off.png");
        public static List<Sprite> SandmanSkin = ModAPIPlus.LimbSprites("Art/AltSkins/Sandminion/");
        public static List<Sprite> SandmanOGSkin = ModAPIPlus.LimbSprites("Art/Skins/Sandman/");

        public static List<Sprite> Loki;

        public static Sprite PowerStone = ModAPI.LoadSprite("Art/Objects/Power Stone.png");
        public static Sprite SpaceStone = ModAPI.LoadSprite("Art/Objects/Space Stone.png");
        public static Sprite RealityStone = ModAPI.LoadSprite("Art/Objects/Reality Stone.png");
        public static Sprite SoulStone = ModAPI.LoadSprite("Art/Objects/Soul Stone.png");
        public static Sprite TimeStone = ModAPI.LoadSprite("Art/Objects/Time Stone.png");
        public static Sprite MindStone = ModAPI.LoadSprite("Art/Objects/Mind Stone.png");

        public static Sprite PowerOn = ModAPI.LoadSprite("Art/Objects/PowerOn.png");
        public static Sprite SpaceOn = ModAPI.LoadSprite("Art/Objects/SpaceOn.png");
        public static Sprite RealityOn = ModAPI.LoadSprite("Art/Objects/RealityOn.png");
        public static Sprite SoulOn = ModAPI.LoadSprite("Art/Objects/SoulOn.png");
        public static Sprite TimeOn = ModAPI.LoadSprite("Art/Objects/TimeOn.png");
        public static Sprite MindOn = ModAPI.LoadSprite("Art/Objects/MindOn.png");

        public static Sprite RepulsorCannon = ModAPI.LoadSprite("Art/Objects/Cannon.png");
        public static Sprite Nanobazooka = ModAPI.LoadSprite("Art/Objects/BazookaLauncher.png");
        public static Sprite NanoBladeS = ModAPI.LoadSprite("Art/Objects/Sword.png");
        public static Sprite NanoShieldS = ModAPI.LoadSprite("Art/Objects/Shield.png");
        public static Sprite NanoHammerS = ModAPI.LoadSprite("Art/Objects/Hammer.png");
        public static Sprite NanoLaser = ModAPI.LoadSprite("Art/Objects/Laser.png");
        public static Sprite NanoLaserGlow = ModAPI.LoadSprite("Art/Objects/Laserglow.png");

        public static Sprite WidowGauntlet = ModAPI.LoadSprite("Art/Objects/WidowGauntlet.png");
        public static Sprite WidowGauntletKnife = ModAPI.LoadSprite("Art/Objects/WidowGauntletSword.png");
        public static Sprite TaserRope = ModAPI.LoadSprite("Art/Objects/Taser.png");

        public static Sprite Vision = ModAPI.LoadSprite("Art/Objects/Vision.png");
        public static Sprite Eye = ModAPI.LoadSprite("Art/Objects/Eye.png");

        public static Sprite Ring = ModAPI.LoadSprite("Art/Objects/Ring.png");
        public static Sprite RingGlow = ModAPI.LoadSprite("Art/Objects/Glow.png");

        //icons
        public static Sprite venomIcon = ModAPI.LoadSprite("Art/UI/Icons/Venom.png");
        public static Sprite Ice = ModAPI.LoadSprite("Art/Objects/Ice.png");
        public static Sprite IceFull = ModAPI.LoadSprite("Art/Objects/IceFull.png");
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
        // fallback thumbnail uses an existing thumbnail to avoid adding binary assets
        public static Sprite DefaultThumbnail = ModAPI.LoadSprite("Art/Thumbnails/Carnage.png");
        public static Sprite ForcefieldSprite = ModAPI.LoadSprite("Art/Objects/Forcefield.png");

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

        public static Texture2D Bifrost = ModAPI.LoadTexture("Art/Objects/Bifrost.png");
        public static Texture2D Whip = ModAPI.LoadTexture("Art/Objects/Whip.png");
        public static Sprite Arrow = ModAPI.LoadSprite("Art/UI/Arrow.png");

        public static Sprite NormalArrow = ModAPI.LoadSprite("Art/Objects/Arrow.png");
        public static Sprite ExplosiveArrow = ModAPI.LoadSprite("Art/Objects/BombArrow.png");
        public static Sprite StunArrow = ModAPI.LoadSprite("Art/Objects/StunArrow.png");
        public static Sprite ElectricArrow = ModAPI.LoadSprite("Art/Objects/ElectroArrow.png");
        public static Sprite PunchArrow = ModAPI.LoadSprite("Art/Objects/PunchArrow.png");


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

        public static AudioClip Snap = ModAPI.LoadSound("Sounds/Snap.wav");

        public static AudioClip Laser = ModAPI.LoadSound("Sounds/Laser.wav");
        public static AudioClip IronLaserSFX = ModAPI.LoadSound("Sounds/IronLaser.wav");
        public static AudioClip LaserStart = ModAPI.LoadSound("Sounds/LaserStart.wav");
        public static AudioClip LaserEnd = ModAPI.LoadSound("Sounds/LaserEnd.wav");
        public static AudioClip AstralBlastSound = ModAPI.LoadSound("Sounds/Soul.wav");
        public static AudioClip Pop = ModAPI.LoadSound("Sounds/Pop.wav");

        public static AudioClip Swap = ModAPI.LoadSound("Sounds/Swap.wav");

        public static AudioClip[] WebSFX =
        {
            ModAPI.LoadSound("Sounds/Web1.wav"),
            ModAPI.LoadSound("Sounds/Web2.wav"),
            ModAPI.LoadSound("Sounds/Web3.wav"),
            ModAPI.LoadSound("Sounds/Web4.wav"),
            ModAPI.LoadSound("Sounds/Web5.wav"),
            ModAPI.LoadSound("Sounds/Web6.wav")
        };


        public static Sprite[] SandBlood =
 {
            ModAPI.LoadSprite("Art/TempForScripts/Sand1.png"),
            ModAPI.LoadSprite("Art/TempForScripts/Sand2.png"),
            ModAPI.LoadSprite("Art/TempForScripts/Sand3.png"),
            ModAPI.LoadSprite("Art/TempForScripts/Sand4.png")
          };

        public static AudioClip ArcBlast = ModAPI.LoadSound("Sounds/Repulsor.wav");
        public static AudioClip ArcBlast2 = ModAPI.LoadSound("Sounds/Repulsor2.wav");
        public static AudioClip ThrusterClip = ModAPI.LoadSound("Sounds/Thruster.wav");

        public static AudioClip NanoTransform = ModAPI.LoadSound("Sounds/Nanotech.wav");
        public static AudioClip NanoGunTransform = ModAPI.LoadSound("Sounds/BlasterTransform.wav");

        public static AudioClip Wind = ModAPI.LoadSound("Sounds/Wind.wav");
        public static AudioClip Winder = ModAPI.LoadSound("Sounds/Winder.wav");
        public static AudioClip Vibrate = ModAPI.LoadSound("Sounds/Vibrate.wav");
        public static AudioClip Cloth = ModAPI.LoadSound("Sounds/Cloth.wav");

        public static AudioClip Hammer = ModAPI.LoadSound("Sounds/Hammer.wav");
        public static AudioClip Bifrosty = ModAPI.LoadSound("Sounds/Bifrosty.wav");
        public static AudioClip Whipy = ModAPI.LoadSound("Sounds/Whip.wav");
        public static AudioClip Teleport = ModAPI.LoadSound("Sounds/Teleport.wav");
        public static AudioClip MagicShoot = ModAPI.LoadSound("Sounds/Projectile.wav");
        public static AudioClip MagicShootWanda = ModAPI.LoadSound("Sounds/WandaShoot.wav");
        public static AudioClip Clock = ModAPI.LoadSound("Sounds/Clock.wav");
        public static AudioClip Bow = ModAPI.LoadSound("Sounds/Bow.wav");
        public static AudioClip BowStart = ModAPI.LoadSound("Sounds/BowStart.wav");
        public static AudioClip Zap = ModAPI.LoadSound("Sounds/Zap.wav");
        #endregion

        #region LoadOther
        public static List<SkinsDictionary> skinsIcons = new List<SkinsDictionary>();
        public static string ModLocation = ModAPI.Metadata.MetaLocation;
        public static GameObject SandImpact;

        #endregion

        #region Structs
        // actions to add to afterspawn that are universal or used very frequently:
        public static Action<GameObject> removeCape = new Action<GameObject>((Instance) =>
        {
            var person = Instance.GetComponent<PersonBehaviour>();

            Destroy(person.Limbs[1].GetComponent<DynamicCape>());
        });
        private static GameObject a;

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
                        limb.gameObject.AddComponent<SpriteMergerAnimatorAdvanced>().Initialize(limb.GetComponent<SpriteRenderer>().sprite, Timtam.GetLimbSprite(skin, limb), limb.GetComponent<Spr[...]
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

        public static UnityEvent IronSkinAddEvent(List<Sprite> skin, PersonBehaviour person, Action<GameObject> addon = null, Sprite Cannon = null, Sprite Hammer = null, Sprite Sword = null, Spri[...]
        {
            var act = new Action<GameObject>((Instance) =>
            {
                var nano = person.GetComponentInChildren<Nanotech>();
                nano.NanotechSuit = skin;

                if (Cannon)
                    foreach (var nanoc in person.GetComponentsInChildren<RepulsorCannons>())
                        nanoc.Repulsor = Cannon;

                if (Hammer)
                    foreach (var nanoc in person.GetComponentsInChildren<NanoHammer>())
                        nanoc.Nanoblade = Hammer;

                if (Sword)
                    foreach (var nanoc in person.GetComponentsInChildren<NanoBlade>())
                        nanoc.Nanoblade = Sword;

                if (Shield)
                    foreach (var nanoc in person.GetComponentsInChildren<NanoShield>())
                        nanoc.Nanoblade = Shield;


                if (nano.transformed)
                {
                    foreach (var limb in person.Limbs)
                    {
                        limb.gameObject.AddComponent<SpriteMergerAnimatorAdvanced>().Initialize(limb.GetComponent<SpriteRenderer>().sprite, Timtam.GetLimbSprite(skin, limb), limb.GetComponent<Spr[...]
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
            public static Action<GameObject> removeCloth(int limb)
            {
                return new Action<GameObject>((Instance) =>
                {
                    var person = Instance.GetComponent<PersonBehaviour>();

                    Destroy(person.Limbs[limb].GetComponent<DynamicCloth>());
                });
            }

            public static Action<GameObject> AddCloth(Sprite cloth, Vector2 PixelPerfectLocation, int limb = 1, Sprite Collar = null)
            {
                var action = new Action<GameObject>((Instance) =>
                {
                    var person = Instance.GetComponent<PersonBehaviour>();

                    DynamicCloth.CreateCloth(person, person.Limbs[limb], cloth.texture, PixelPerfectLocation * ModAPI.PixelSize);

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

            public static GameObject AddGlowToObject(GameObject parent, Sprite sprite)
            {
                var glowObject = new GameObject("Glow");
                glowObject.transform.parent = parent.transform;
                glowObject.transform.localPosition = Vector3.zero;
                glowObject.transform.localRotation = Quaternion.identity;
                glowObject.transform.localScale = Vector3.one;

                var parentSr = parent.GetComponent<SpriteRenderer>();

                var sr = glowObject.AddComponent<SpriteRenderer>();
                sr.sprite = sprite;
                sr.gameObject.layer = 9;
                sr.sortingLayerName = parentSr.sortingLayerName;
                sr.sortingOrder = parentSr.sortingOrder + 1;
                sr.color = Color.white;
                sr.material = ModAPI.FindMaterial("VeryBright");

                glowObject.AddComponent<SortingLayerChild>().Behaviour = SortingLayerChild.SortBehaviour.Above;
                return glowObject;
            }

            public static IEnumerator PlaySound(Vector2 position, AudioClip SoundToPlay, float pitch = 1, float volume = 1)
            {
                var sound = new GameObject();
                sound.name = "SFX";
                sound.transform.position = position;
                sound.AddComponent<AudioSource>();
                sound.GetComponent<AudioSource>().playOnAwake = false;
                sound.GetComponent<AudioSource>().clip = SoundToPlay;
                sound.GetComponent<AudioSource>().spatialBlend = 1;
                sound.GetComponent<AudioSource>().volume = volume;
                sound.GetComponent<AudioSource>().pitch = pitch;
                sound.AddComponent<AudioDistortionFilter>().distortionLevel = 0.85f;
                sound.AddComponent<AudioSourceTimeScaleBehaviour>();
                sound.GetComponent<AudioSource>().Play();
                sound.AddComponent<OBJDestroyer>();

                yield return new WaitForSeconds(SoundToPlay.length);

                Destroy(sound);
            }

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

                string[] spriteNames = { "Head", "UpperBody", "MiddleBody", "LowerBody", "UpperArmFront", "LowerArmFront", "UpperArm", "LowerArm", "UpperLegFront", "LowerLegFront", "FootFront", "[...," };

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

            public static List<Sprite> LimbGlowSprites(string FilePath)
            {
                List<Sprite> limbSprites = new List<Sprite>();

                string[] spriteNames = { "HeadG", "UpperBodyG", "MiddleBodyG", "LowerBodyG", "UpperArmFrontG", "LowerArmFrontG", "UpperArmG", "LowerArmG", "UpperLegFrontG", "LowerLegFrontG", "Foo[..." };

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

            public static Sprite GetThumbnailOrDefault(string Thumbname)
            {
                if (!string.IsNullOrEmpty(Thumbname))
                {
                    try
                    {
                        var s = ModAPI.LoadSprite("Art/Thumbnails/" + Thumbname + ".png");
                        if (s != null) return s;
                    }
                    catch { }
                }
                return Mod.DefaultThumbnail;
            }

            public static ItemButtonBehaviour GetIcon(string spawnableAsset)
            {
                foreach (ItemButtonBehaviour item in GameObject.FindObjectsOfType<ItemButtonBehaviour>(true)) { if (item.Item != null) if (item.Item.name == spawnableAsset) return item; }
                return null;
            }

            public static void CreateHuman(string name, string description, string FileName, string Thumbname, Action<GameObject> AfterSpawn = null, string OrderOverride = null, Sprite Flesh = nu[...]
            {
                Sprite thumbSprite = GetThumbnailOrDefault(Thumbname);

                var bob = new SkinsDictionary()
                {
                    characterName = "[" + CategoryName + "] " + name
                };
                if (thumbSprite != null) bob.icons.Add(thumbSprite);
                Mod.skinsIcons.Add(bob);
                ModAPI.Register(
                    new Modification()
                    {
                        OriginalItem = ModAPI.FindSpawnable("Human"),
                        NameOverride = "[" + CategoryName + "] " + name,
                        NameToOrderByOverride = OrderOverride,
                        DescriptionOverride = description,
                        CategoryOverride = ModAPI.FindCategory(CategoryName),
                        ThumbnailOverride = thumbSprite,
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

                            UnityEvent a = new UnityEvent();
                            a.AddListener(() =>
                            {
                                if (SkinAdd != null)
                                    SkinAdd.Invoke(Instance);
                            });

                            UnityEvent b = new UnityEvent();
                            b.AddListener(() =>
                            {
                                if (SkinRemove != null)
                                    SkinRemove.Invoke(Instance);
                            });
                            menu.AddButton("Default", thumbSprite, ModAPIPlus.LimbSprites("Art/Skins/" + FileName + "/"), a, b, thumbSprite);

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

            public static void CreateAndroid(string name, string description, string FileName, string Thumbname, Action<GameObject> AfterSpawn = null, string OrderOverride = null)
            {
                Sprite thumbSprite = GetThumbnailOrDefault(Thumbname);

                var bob = new SkinsDictionary()
                {
                    characterName = "[" + CategoryName + "] " + name
                };
                if (thumbSprite != null) bob.icons.Add(thumbSprite);
                Mod.skinsIcons.Add(bob);
                ModAPI.Register(
                    new Modification()
                    {
                        OriginalItem = ModAPI.FindSpawnable("Android"),
                        NameOverride = "[" + CategoryName + "] " + name,
                        NameToOrderByOverride = OrderOverride,
                        DescriptionOverride = description,
                        CategoryOverride = ModAPI.FindCategory(CategoryName),
                        ThumbnailOverride = thumbSprite,
                        AfterSpawn = (Instance) =>
                        {
                            var person = Instance.GetComponent<PersonBehaviour>();
                            var skin = ModAPI.LoadTexture("Art/Skins/" + FileName + "/Skin.png");
                            person.SetBodyTextures(skin);

                            var menu = Instance.AddComponent<TextureMenu>();
                            menu.CreateUI();
                            menu.AddButton("Default", thumbSprite, ModAPIPlus.LimbSprites("Art/Skins/" + FileName + "/"), null, null, thumbSprite);

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

            public static void CreateObject(string originalObject, string name, string description, string FileName, string Thumbname, Action<GameObject> AfterSpawn = null, string OrderOverride =[...]
            {
                Sprite thumbSprite = GetThumbnailOrDefault(Thumbname);

                ModAPI.Register(
                    new Modification()
                    {
                        OriginalItem = ModAPI.FindSpawnable(originalObject),
                        NameOverride = "[" + CategoryName + "] " + name,
                        NameToOrderByOverride = OrderOverride,
                        DescriptionOverride = description,
                        CategoryOverride = ModAPI.FindCategory(CategoryName),
                        ThumbnailOverride = thumbSprite,
                        AfterSpawn = (Instance) =>
                        {
                            if (Instance.GetComponent<SpriteRenderer>() && Instance.GetComponent<PhysicalBehaviour>())
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

            #region Checks
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

                        DialogBoxManager.Dialog("Floating Bug Fix creates errors with " + CategoryName + ", do you want to disable this mod?", button1, button2).transform.localPosition += new Vec[...]
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
            #endregion

            #region Settings
            Settings.main = new Settings();
            Settings.main.AddSetting("UI Size", "Adjust the UI scale if it is incorrect", "UISize", 0.9f, typeof(float), 0.5f, 1.5f);
            Settings.main.AddSetting("Capes", "When disabled, no characters will spawn with capes", "UseCapes", true, typeof(bool));
            Settings.main.AddSetting("Legacy Mass System", "When enabled, it will force all 'stronger' characters masses to be 0.5, negating issues with characters jumping around too much", "Lega[...]
            Settings.main.AddSetting("Stronger healing", "Automatically enables the Speed Healing power on entities using the slower varient.", "SpeedHeal", false, typeof(bool));
            Settings.main.AddSetting("Simple Timestop", "Heavily reduces lag when using timestop at the cost of some affects being removed.", "SimpleTimestop", false, typeof(bool));
            Settings.main.AddSetting("Slow Motion Speed", "Adjust the slow motion speed", "SlowMotionSpeed", 0.1f, typeof(float), 0.01f, 0.1f);
            Settings.main.AddSetting("Flight Stun", "Adjust the duration that a character is stunned when hit while flying", "KnockoutTime", 1.5f, typeof(float), 0f, 5f);
            Settings.main.AddSetting("Durability", "Changes the damage dampening on speed healing and super strength, divides impact damage variable by whatever is inputted in this setting, and s[...]
            Settings.main.AddSetting("Hammer Worthiness", "Toggle whether thor's hammer should only be able to be picked up by worthy users!", "HammerWorthiness", true, typeof(bool));
            Settings.main.AddSetting("Ultron Body Swapping", "Toggles whether Ultron can swap bodies when killed automatically", "UltronSwapping", true, typeof(bool));
            Settings.main.AddSetting("Default Web Type", "(1 = normal, 2 = connector, 3 = grapple, 4 = electric, 5 = webshot, 6 = none)", "DefaultWeb", 1, typeof(int), 1, 6);
            Settings.main.AddSetting("Synced Web Type", "When enabled, it will make all limbs with the web ability use the same web type when one is changed", "SyncedWeb", false, typeof(bool));
            Settings.main.AddSetting("Rigid Webs", "Will change the joint type of the webs from being springy to stiff.", "RigidWebs", false, typeof(bool));
            Settings.main.AddSetting("Rigid Web Strength", "Adjusts the strength a rigid joint can take before breaking.", "RigidWebStrength", 5000, typeof(int));
            Settings.main.AddSetting("Web Length", "Adjust the length a web can shoot from.", "WebLength", 25f, typeof(float));
            Settings.main.AddSetting("Webshot force", "Adjust the force a webshot is shot at.", "WebshotForce", 10f, typeof(float));
            Settings.main.AddSetting("Wall Climbing Force", "Adjust the strength that characters stick to walls.", "WallClimbForce", 500, typeof(int));
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
            Portaller.PortalPrefab = ABloader.LoadFromAB<GameObject>(ABloader.LoadFromFile("AssetBundles/portal"), "portal");
            MagicProjectile.ProjectilePrefab = ABloader.LoadFromAB<GameObject>(ABloader.LoadFromFile("AssetBundles/portal"), "Projectile");
            Telekinesis.RunePrefab = ABloader.LoadFromAB<GameObject>(ABloader.LoadFromFile("AssetBundles/portal"), "Rune");
            TimeFreeze.RunePrefab = ABloader.LoadFromAB<GameObject>(ABloader.LoadFromFile("AssetBundles/portal"), "TimeRune");
            AstralFist.SoulRune = ABloader.LoadFromAB<GameObject>(ABloader.LoadFromFile("AssetBundles/portal"), "SoulRune");
            LifeDrain.RunePrefab = ABloader.LoadFromAB<GameObject>(ABloader.LoadFromFile("AssetBundles/portal"), "Drain");
            LifeDrain.ExplodePrefab = ABloader.LoadFromAB<GameObject>(ABloader.LoadFromFile("AssetBundles/portal"), "WandaExplosion");
            WandaAreaBlast.BlastPrefab = ABloader.LoadFromAB<GameObject>(ABloader.LoadFromFile("AssetBundles/portal"), "Area");
            WandaProjectile.ProjectilePrefab = ABloader.LoadFromAB<GameObject>(ABloader.LoadFromFile("AssetBundles/portal"), "WandaProjectile");
            WandaAreaBlast.DamageVictim.PFXPrefab = ABloader.LoadFromAB<GameObject>(ABloader.LoadFromFile("AssetBundles/portal"), "Damage");
            SpacePortals.PortalPrefab = ABloader.LoadFromAB<GameObject>(ABloader.LoadFromFile("AssetBundles/portal"), "Teleportal");
            InfinityGauntlet.MenuPrefab = ABloader.LoadFromAB<GameObject>(ABloader.LoadFromFile("AssetBundles/Gauntlet"), "Gauntlet Menu");
            #endregion
        }

        #endregion

        public static void Main()
        {
            //Category
            ModAPI.RegisterCategory(CategoryName, "Category for Nova's Avengers Mod", ModAPI.LoadSprite("icon.png"));

            #region Heros

            //Tony Stark
            ModAPIPlus.CreateHuman("Tony Stark (Iron Man)", "", "Tony Stark", "Iron Man", (Instance) =>
            {
                var person = Instance.GetComponent<PersonBehaviour>();
      
                var menu = Instance.GetComponent<TextureMenu>();

                menu.AddButton("Mcu", ModAPI.LoadSprite("Art/Thumbnails/Mcu Tony.png"), ModAPIPlus.LimbSprites("Art/AltSkins/Mcu Tony/"));

                Nanotech.SetPower(person, person.Limbs[0], ModAPI.LoadSprite("Art/UI/Icons/Nanotech.png")).EnablePower();
                SlowHealing.SetPower(person, ModAPI.LoadSprite("Art/UI/Icons/Heal.png")).EnablePower();

                void IronSkin(string Name, string Display = null, string WeaponFolder = null)
                {
                    Sprite cannon = null;
                    Sprite hammer = null;
                    Sprite sword = null;
                    Sprite shield = null;
                    Sprite laser = null;

                    if (WeaponFolder != null)
                    {
                        cannon = ModAPI.LoadSprite("Art/Objects/" + WeaponFolder + "/Cannon.png");
                        hammer = ModAPI.LoadSprite("Art/Objects/" + WeaponFolder + "/Hammer.png");
                        sword = ModAPI.LoadSprite("Art/Objects/" + WeaponFolder + "/Sword.png");
                        shield = ModAPI.LoadSprite("Art/Objects/" + WeaponFolder + "/Shield.png");
                        laser = ModAPI.LoadSprite("Art/Objects/" + WeaponFolder + "/Laser.png");
                    }else
                    {
                        cannon = ModAPI.LoadSprite("Art/Objects/Cannon.png");
                        hammer = ModAPI.LoadSprite("Art/Objects/Hammer.png");
                        sword = ModAPI.LoadSprite("Art/Objects/Sword.png");
                        shield = ModAPI.LoadSprite("Art/Objects/Shield.png");
                        laser = ModAPI.LoadSprite("Art/Objects/Laser.png");
                    }

... (file continues unchanged) ...
