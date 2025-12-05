using Hints;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMOD.API.CustHint
{
    public class CustomHint : FormattableHint<CustomHint>
    {
        public delegate string AutoTextDelegate(float progress);

        public static CustomHint FromNetwork(NetworkReader reader)
        {
            CustomHint hint = new CustomHint();
            hint.Deserialize(reader);
            return hint;
        }

        public int YCoordinate { get; private set; }
        public int XCoordinate { get; private set; }
        public float ShowTime { get; private set; }

        [System.NonSerialized]
        private AutoTextDelegate _autoText;
        public AutoTextDelegate AutoText
        {
            get => _autoText;
            private set => _autoText = value;
        }

        private List<HintEffect> _hintEffects;
        public IReadOnlyList<HintEffect> HintEffects => _hintEffects?.AsReadOnly() ?? new List<HintEffect>().AsReadOnly();

        private CustomHint() : base(null, null, 1f)
        {
            _hintEffects = new List<HintEffect>();
        }

        public CustomHint(
            int xCoordinate,
            int yCoordinate,
            float showTime = 1f,
            AutoTextDelegate autoText = null,
            List<HintEffect> hintEffects = null,
            HintParameter[] parameters = null,
            float durationScalar = 1f)
            : base(parameters, null, durationScalar)
        {
            this.XCoordinate = xCoordinate;
            this.YCoordinate = yCoordinate;
            this.ShowTime = showTime;
            this.AutoText = autoText;
            this._hintEffects = hintEffects ?? new List<HintEffect>();

            if (autoText != null)
            {
                _autoText = autoText;
            }
        }

        public string GetDisplayText(float progress)
        {
            if (_autoText != null)
            {
                try
                {
                    return _autoText(progress);
                }
                catch (Exception ex)
                {
                }
            }

            if (this.Parameters != null && this.Parameters.Length > 0)
            {
                return FormatWithParameters(progress);
            }

            return $"位置: ({XCoordinate}, {YCoordinate}) | 剩余时间: {ShowTime * (1 - progress):F1}s";
        }

        private string FormatWithParameters(float progress)
        {
            var formattedParts = new List<string>();
            foreach (var param in this.Parameters)
            {
                if (param.Update(progress))
                {
                    formattedParts.Add(param.Formatted);
                }
            }

            return string.Join(" | ", formattedParts);
        }

        public override void Deserialize(NetworkReader reader)
        {
            base.Deserialize(reader);

            this.XCoordinate = reader.ReadInt();
            this.YCoordinate = reader.ReadInt();
            this.ShowTime = reader.ReadFloat();

            int effectCount = reader.ReadInt();
            _hintEffects = new List<HintEffect>();
            for (int i = 0; i < effectCount; i++)
            {
                string effectType = reader.ReadString();
                HintEffect effect = CreateEffectFromType(effectType, reader);
                if (effect != null)
                {
                    _hintEffects.Add(effect);
                }
            }

            this.AutoText = null;
        }

        public override void Serialize(NetworkWriter writer)
        {
            base.Serialize(writer);

            writer.WriteInt(this.XCoordinate);
            writer.WriteInt(this.YCoordinate);
            writer.WriteFloat(this.ShowTime);

            writer.WriteInt(_hintEffects.Count);
            foreach (var effect in _hintEffects)
            {
                writer.WriteString(effect.GetType().Name);
                effect.Serialize(writer);
            }
        }

        private HintEffect CreateEffectFromType(string typeName, NetworkReader reader)
        {
            switch (typeName)
            {
                case nameof(AlphaEffect):
                    return AlphaEffect.FromNetwork(reader);
                default:
                    return null;
            }
        }

        public void AddEffect(HintEffect effect)
        {
            if (effect != null)
            {
                _hintEffects.Add(effect);
            }
        }

        public bool RemoveEffect(HintEffect effect)
        {
            return _hintEffects.Remove(effect);
        }

        public void ClearEffects()
        {
            _hintEffects.Clear();
        }

        public static CustomHint CreateWithCoordinateParameters(int x, int y, float showTime = 1f,
            AutoTextDelegate autoText = null, List<HintEffect> effects = null)
        {
            var parameters = new HintParameter[]
            {
                new StringHintParameter($"位置: ({x}, {y})"),
                new Hints.TimespanHintParameter(showTime, "剩余: {0}", false)
            };

            return new CustomHint(x, y, showTime, autoText, effects, parameters);
        }
    }
}
