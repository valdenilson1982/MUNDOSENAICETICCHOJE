using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomCharacter
{
    public class RandomCharacterInstancer : RandomInstanceGenerator
    {
        public enum ColliderType
        {
            None,
            CapsuleCollider,
            BoxCollider
        }

        public ColliderType colliderType = ColliderType.None;
        public Vector3 size = Vector3.one, center = Vector3.zero;
        public float radius = 1, height = 2;
        public bool isTrigger = false;
        public PhysicMaterial material;

        public int characterLayer;
        public string characterTag = "Untagged";

        public Enumerations.characterGenre genre = Enumerations.characterGenre.Random;
        public Enumerations.desition haveBear = Enumerations.desition.Random;
        public Enumerations.desition useMask = Enumerations.desition.Random;
        public Enumerations.desition useBackPack = Enumerations.desition.Random;
        public Enumerations.desition useHelmet = Enumerations.desition.Random;
        public Enumerations.desition useArmourChest = Enumerations.desition.Random;
        public Enumerations.desition useShouldersProtection = Enumerations.desition.Random;
        public Enumerations.desition useWristsProtection = Enumerations.desition.Random;
        public Enumerations.desition useThighsProtection = Enumerations.desition.Random;
        public Enumerations.desition useKneeProtection = Enumerations.desition.Random;
        public Enumerations.desition hasWeapon = Enumerations.desition.Random;

        public Enumerations.desition useGloves = Enumerations.desition.Random;
        public Enumerations.desition useBoots = Enumerations.desition.Random;
        public Enumerations.desition useShirt = Enumerations.desition.Random;
        public Enumerations.desition usePants = Enumerations.desition.Random;

        public bool useRandomRotation = false;
        public bool placeUnparent = false;

        public bool useDistanciation = false;
        public float checkRadius = 5;
        public int attemts = 3;

        public override void Generate()
        {
            characterSet.Init();

            characterSet.genre = this.genre;
            characterSet.haveBear = this.haveBear;
            characterSet.useMask = this.useMask;
            characterSet.useBackPack = this.useBackPack;
            characterSet.useHelmet = this.useHelmet;
            characterSet.useArmourChest = this.useArmourChest;
            characterSet.useShouldersProtection = this.useShouldersProtection;
            characterSet.useWristsProtection = this.useWristsProtection;
            characterSet.useThighsProtection = this.useThighsProtection;
            characterSet.useKneeProtection = this.useKneeProtection;
            characterSet.hasWeapon = this.hasWeapon;

            characterSet.useGloves = this.useGloves;
            characterSet.useShirt = this.useShirt;
            characterSet.usePants = this.usePants;
            characterSet.useBoots = this.useBoots;

            if (!placeUnparent)
            {
                GameObject character = characterSet.Generate(transform);

                if (!character)
                    return;

                character.transform.localScale = Vector3.one * Random.Range(characterSet.minSize, characterSet.maxSize);
                character.transform.position = transform.position;

                if (characterSet.selectedModule.overrideLayer)
                {
                    character.layer = characterSet.selectedModule.layer;
                }
                else
                {
                    character.layer = characterLayer;
                }

                if (characterSet.selectedModule.overrideTag)
                {
                    character.tag = characterSet.selectedModule.tag;
                }
                else
                {
                    character.tag = characterTag;
                }

                switch (colliderType)
                {
                    case ColliderType.BoxCollider:
                        BoxCollider box = character.AddComponent<BoxCollider>();
                        box.isTrigger = isTrigger;
                        box.material = material;
                        box.size = size;
                        box.center = center;
                        break;

                    case ColliderType.CapsuleCollider:
                        CapsuleCollider capsule = character.AddComponent<CapsuleCollider>();
                        capsule.isTrigger = isTrigger;
                        capsule.material = material;
                        capsule.center = center;
                        capsule.radius = radius;
                        capsule.height = height;
                        break;
                }
            }
            else
            {
                GameObject character = characterSet.Generate(transform.position, useRandomRotation);

                if (!character)
                    return;

                character.transform.localScale = Vector3.one * Random.Range(characterSet.minSize, characterSet.maxSize);
                character.transform.position = transform.position;

                if (characterSet.selectedModule.overrideLayer)
                {
                    character.layer = characterSet.selectedModule.layer;
                }
                else
                {
                    character.layer = characterLayer;
                }

                if (characterSet.selectedModule.overrideTag)
                {
                    character.tag = characterSet.selectedModule.tag;
                }
                else
                {
                    character.tag = characterTag;
                }

                switch (colliderType)
                {
                    case ColliderType.BoxCollider:
                        BoxCollider box = character.AddComponent<BoxCollider>();
                        box.isTrigger = isTrigger;
                        box.material = material;
                        box.size = size;
                        box.center = center;
                        break;

                    case ColliderType.CapsuleCollider:
                        CapsuleCollider capsule = character.AddComponent<CapsuleCollider>();
                        capsule.isTrigger = isTrigger;
                        capsule.material = material;
                        capsule.center = center;
                        capsule.radius = radius;
                        capsule.height = height;
                        break;
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawIcon(transform.position, "SingleInstancer_Icon.tiff", true);

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position + Vector3.up, new Vector3(1, 2, 1));
        }
    }
}
