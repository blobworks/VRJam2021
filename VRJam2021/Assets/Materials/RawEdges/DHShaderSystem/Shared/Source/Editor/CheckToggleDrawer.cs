using UnityEngine;
using UnityEditor;

namespace DHShaderSystem
{
    public class CheckToggleDrawer : MaterialPropertyDrawer
    {
        protected bool isArgumentGiven;
        protected string argValue;

        public CheckToggleDrawer()
        {
            isArgumentGiven = false;
            argValue = "";
        }

        public CheckToggleDrawer(string arg)
        {
            isArgumentGiven = true;
            argValue = arg;
        }

        public override void OnGUI(Rect position, MaterialProperty prop, string label, MaterialEditor editor)
        {
            if (isArgumentGiven)
            {
                Material material = editor.target as Material;

                if (material != null)
                {
                    if (material.IsKeywordEnabled(argValue) == false)
                    {
                        return;
                    }
                }
            }

            editor.DefaultShaderProperty(position, prop, label);
        }

        public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor)
        {
            if (isArgumentGiven)
            {
                Material material = editor.target as Material;

                if (material != null)
                {
                    if (material.IsKeywordEnabled(argValue) == false)
                    {
                        return 0;
                    }
                }
            }

            // Magic code. Texture property height is wrong
            if (prop.type == MaterialProperty.PropType.Texture)
                return 68;

            return base.GetPropertyHeight(prop, label, editor);
        }
    }
}