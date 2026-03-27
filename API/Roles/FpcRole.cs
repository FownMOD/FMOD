using CursorManagement;
using PlayerRoles;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.FirstPersonControl.Thirdperson;
using PlayerRoles.Visibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FMOD.API.Roles
{
    public class FpcRole : Role
    {
        public FpcRole(PlayerRoleBase roleBase) : base(roleBase)
        {
            this.Base = (FpcStandardRoleBase)roleBase;
        }
        public new FpcStandardRoleBase Base {  get; set; }
        public Transform GetBodyTransform(HumanBodyBones humanBodyBones)
        {
            AnimatedCharacterModel characterModel = Model as AnimatedCharacterModel;
            Animator componentInChildren = characterModel.gameObject.GetComponentInChildren<Animator>();
            return componentInChildren.GetBoneTransform(humanBodyBones);
        }
        public CharacterModel Model
        {
            get
            {
                return this.Base.FpcModule.CharacterModelInstance;
            }
        }
        public Vector3 Gravity
        {
            get
            {
                return this.Base.FpcModule.Motor.GravityController.Gravity;
            }
            set
            {
                this.Base.FpcModule.Motor.GravityController.Gravity = value;
            }
        }
        public FpcNoclip FpcNoclip
        {
            get
            {
                return Base.FpcModule.Noclip;
            }
        }
        public VisibilityController VisibilityController => Base.VisibilityController;
        public static List<Player> VisibilityList = new List<Player>();
    }
}
