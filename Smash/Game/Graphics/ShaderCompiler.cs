using Smash.Core.Graphics;
using Smash.Game.Asset;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smash.Game.Graphics
{
    public static class ShaderCompiler
    {
        public static Dictionary<string, string> Definitions = new Dictionary<string, string>()
        {
            {"VERTEX_IN", @"
layout (location = 0) in vec2 TextCoord0;
layout (location = 1) in vec2 TextCoord1;
layout (location = 2) in vec3 Position;
layout (location = 3) in vec3 Normal;
layout (location = 4) in vec3 Tangent;
layout (location = 5) in vec3 BiTangent;
layout (location = 6) in vec4 Color0;
layout (location = 7) in vec4 Color1;
layout (location = 8) in vec4 Weights;
layout (location = 9) in ivec4 WeightIndex;

out vec2 textCoord0;
out vec2 textCoord1;
out vec3 position;
out vec3 normal;
out vec3 tangent;
out vec3 biTangent;
out vec4 color0;
out vec4 color1;
out vec4 weights;

layout (std140) uniform SKEL_NAME
{   
    mat4 SkeletonData[300];
    mat4 CameraView;
};

layout (std140) uniform SceneData
{
    mat4 Camera;
    mat4 Transforms[1000];
};

mat4 GetSkinnedTransform(bool IsDirection)
{
    mat4 _transform = mat4(0);

    for (int i = 0; i < 4; ++i)
    {
        int index = WeightIndex[i];
        float weight = Weights[i];

        _transform += IsDirection ? transpose(inverse(SkeletonData[index])) * weight : (SkeletonData[index] * weight);
    }

    return _transform;
};

vec3 TransformPoint(mat4 transform, vec3 point)
{
    return (transform * vec4(point, 1)).xyz;
}
".Replace("SKEL_NAME", Skeleton.UBOName)},

            {
                "FRAGMENT_IN", @"
out vec4 fragColor;

in vec2 textCoord0;
in vec2 textCoord1;
in vec3 position;
in vec3 normal;
in vec3 tangent;
in vec3 biTangent;
in vec4 color0;
in vec4 color1;
in vec4 weights;

uniform sampler2D Texture0;
uniform sampler2D Texture1;
uniform sampler2D Texture2;
uniform sampler2D Texture3;
uniform sampler2D Texture4;
uniform sampler2D Texture5;
uniform sampler2D Texture6;
uniform sampler2D Texture7;
"
            }

        };

        public static string FillShader(string source)
        {
            string Out = source;

            string[] Keys = Definitions.Keys.ToArray();

            foreach (string key in Keys)
            {
                Out = Out.Replace($"#include {key}", Definitions[key]);
            }

            return Out;
        }
    }
}
