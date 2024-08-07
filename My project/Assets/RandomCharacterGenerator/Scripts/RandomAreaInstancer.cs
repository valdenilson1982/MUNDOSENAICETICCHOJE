using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomCharacter {
    public class RandomAreaInstancer : RandomInstanceGenerator
    {
        public enum ColliderType
        {
            None,
            CapsuleCollider,
            BoxCollider
        }
        public enum SpawnMethod
        {
            FixedPosition,
            RandomPosition
        }

        public ColliderType colliderType = ColliderType.None;
        public Vector3 size = Vector3.one, center = Vector3.zero;
        public float radius = 1, height = 2;
        public bool isTrigger = false;
        public PhysicMaterial material;

        public int characterLayer;
        public string characterTag = "Untagged";

        public bool spawnAtPosition;
        public Transform[] spawnPositions = new Transform[0];
        public Transform[] tempSpawnPositions;

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

        public LayerMask positioningMask;
        public Vector2 zone = new Vector2(5, 5);
        public bool useRandomRotation = false;
        public bool placeUnparent = false;

        public bool useDistanciation = false;
        public float checkRadius = 5;
        public int attemts = 3;

        public SpawnMethod spawnMethod = SpawnMethod.RandomPosition;

        public override IEnumerator GenerateOnIntervals()
        {
            characterSet.Init();
            yield return new WaitForSeconds(startDelay);
            while (true)
            {
                switch (spawnMethod)
                {
                    case SpawnMethod.FixedPosition:
                        Transform sPos = spawnPositions[Random.Range(0, spawnPositions.Length)];
                        if (sPos)
                        {
                            GameObject character = characterSet.Generate(sPos.position, useRandomRotation);
                            character.transform.localScale = Vector3.one * Random.Range(characterSet.minSize, characterSet.maxSize);

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
                        break;

                    case SpawnMethod.RandomPosition:
                        float randomX = Random.Range(-zone.x / 2, zone.x / 2);
                        float randomZ = Random.Range(-zone.y / 2, zone.y / 2);

                        Ray ray = new Ray(transform.position + new Vector3(randomX, 1000, randomZ), Vector3.down); ;

                        RaycastHit hit;
                        if (!Physics.Raycast(ray, out hit, float.MaxValue, characterSet.obstacleMask))
                        {
                            if (Physics.Raycast(ray, out hit, float.MaxValue, characterSet.positioningMask))
                            {
                                GameObject character = characterSet.Generate(hit.point, useRandomRotation);
                                character.transform.localScale = Vector3.one * Random.Range(characterSet.minSize, characterSet.maxSize);
                                character.transform.position = hit.point;

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
                        break;
                }
                yield return new WaitForSeconds(generationInterval);

            }
        }

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

            switch (spawnMethod)
            {
                case SpawnMethod.FixedPosition:
                    for (int i = 0; i < spawnPositions.Length; i++)
                    {
                        if (spawnPositions[i])
                        {
                            spawnPositions[i].name = "New Character " + (i + 1).ToString();

                            if (!placeUnparent)
                            {
                                GameObject character = characterSet.Generate(spawnPositions[i].transform);
                                character.transform.localScale = Vector3.one * Random.Range(characterSet.minSize, characterSet.maxSize);

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
                                GameObject character = characterSet.Generate(spawnPositions[i].position, useRandomRotation);
                                character.transform.localScale = Vector3.one * Random.Range(characterSet.minSize, characterSet.maxSize);

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
                    }
                    break;

                case SpawnMethod.RandomPosition:
                    for (int i = 0; i < amount; i++)
                    {
                        float randomX = Random.Range(-zone.x / 2, zone.x / 2);
                        float randomZ = Random.Range(-zone.y / 2, zone.y / 2);

                        Ray ray = new Ray(transform.position + new Vector3(randomX, 1000, randomZ), Vector3.down); ;

                        RaycastHit hit;
                        if (!Physics.Raycast(ray, out hit, float.MaxValue, characterSet.obstacleMask))
                        {
                            if (Physics.Raycast(ray, out hit, float.MaxValue, characterSet.positioningMask))
                            {
                                if (useDistanciation)
                                {
                                    bool aviable = true;

                                    for (int a = 0; a < attemts; a++)
                                    {
                                        aviable = !Physics.CheckSphere(hit.point, radius, characterSet.checkMask);
                                        if (aviable)
                                        {
                                            break;
                                        }
                                    }

                                    if (!aviable)
                                    {
                                        continue;
                                    }
                                }

                                GameObject character = characterSet.Generate(hit.point, useRandomRotation);

                                if (!character)
                                    return;

                                character.transform.localScale = Vector3.one * Random.Range(characterSet.minSize, characterSet.maxSize);
                                character.transform.position = hit.point;

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
                    }

                    break;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawIcon(transform.position, "Instancer_Icon.tiff", true);

            if (spawnMethod == SpawnMethod.RandomPosition)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(transform.position + Vector3.up * 3, new Vector3(zone.x, 6, zone.y));
            }

            if (spawnMethod == SpawnMethod.FixedPosition)
            {
                foreach (Transform t in spawnPositions)
                {
                    if (t)
                    {
                        Gizmos.color = Color.green;
                        Gizmos.DrawWireCube(t.position + Vector3.up, new Vector3(1, 2, 1));
                    }
                }
            }
        }
    }
}
