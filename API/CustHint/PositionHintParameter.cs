using Hints;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.API.CustHint
{
    public class PositionHintParameter : HintParameter
    {
        private float _x;
        private float _y;

        public PositionHintParameter(float x, float y)
        {
            _x = x;
            _y = y;
        }

        protected override string UpdateState(float progress)
        {
            return $"pos:{_x}:{_y}";
        }

        public override void Serialize(NetworkWriter writer)
        {
            Serialize(writer);
            writer.WriteFloat(_x);
            writer.WriteFloat(_y);
        }

        public override void Deserialize(NetworkReader reader)
        {
            Deserialize(reader);
            _x = reader.ReadFloat();
            _y = reader.ReadFloat();
        }
    }
}
