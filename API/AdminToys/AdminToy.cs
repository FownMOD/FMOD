using AdminToys;
using FMOD.Enums;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FMOD.API.AdminToys
{
    public abstract class AdminToy
    {
        public abstract AdminToyType AdminToyType { get; }

        public static Dictionary<AdminToyBase, AdminToy> Dictionary = new Dictionary<AdminToyBase, AdminToy>();

        public AdminToyBase Base { get; set; }

        public AdminToy(AdminToyBase adminToyBase)
        {
            this.Base = adminToyBase;
            if (!Dictionary.ContainsKey(adminToyBase))
            {
                Dictionary.Add(adminToyBase, this);
            }
        }

        public static AdminToy Get(AdminToyBase adminToyBase)
        {
            if (Dictionary.ContainsKey(adminToyBase))
                return Dictionary[adminToyBase];
            return null;
        }

        public static bool TryGet(AdminToyBase adminToyBase, out AdminToy adminToy)
        {
            adminToy = Get(adminToyBase);
            return adminToy != null;
        }

        public static AdminToy Create(AdminToyType adminToyType, Vector3 pos)
        {
            switch (adminToyType)
            {
                case AdminToyType.Camera:
                    return CameraToy.Create(pos);
                case AdminToyType.Capybara:
                    return Capybara.Create(pos);
                case AdminToyType.Interactable:
                    return InteractableToy.Create(pos);
                case AdminToyType.Light:
                    return LightToy.Create(pos, 10f);
                case AdminToyType.Primitive:
                    return Primitive.Create(pos);
                case AdminToyType.ShootingTarget:
                    return ShootingTargetToy.Create(pos);
                case AdminToyType.Speaker:
                    return Speaker.Create(pos);
                case AdminToyType.Text:
                    return Text.Create(pos);
                case AdminToyType.Waypoint:
                    return Waypoint.Create(pos);
                default:
                    throw new NotImplementedException($"AdminToyType {adminToyType} not implemented");
            }
        }
        protected static GameObject FindPrefab<T>() where T : AdminToyBase
        {
            foreach (var prefab in NetworkManager.singleton.spawnPrefabs)
            {
                if (prefab.GetComponent<T>() != null)
                    return prefab;
            }
            return null;
        }

        public Vector3 Scale
        {
            get => Base.Scale;
            set => Base.Scale = value;
        }

        public Vector3 Position
        {
            get => Base.NetworkPosition;
            set => Base.NetworkPosition = value;
        }

        public Quaternion Rotation
        {
            get => Base.NetworkRotation;
            set => Base.NetworkRotation = value;
        }

        public void Destroy()
        {
            if (Base != null && Base.gameObject != null)
            {
                NetworkServer.Destroy(Base.gameObject);
                Dictionary.Remove(Base);
            }
        }
    }
}