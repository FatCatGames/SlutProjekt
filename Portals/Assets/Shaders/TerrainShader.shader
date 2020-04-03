Shader "Synthwave/Terrain"
{
    Properties
    {
        [HDR]_FloorColor ("Low color", Color) = (0, 0, 1, 1)
        [HDR]_MountainColor ("High color", Color) = (1, 0, 1, 1)
        _BackgroundColor ("Background color", Color) = (0,0,0,1)
        _OutlineSize ("Size", Range (0.8, 1)) = 0.9933
        _FadeDist ("Distance at which wireframe should fade to background", Float) = 30
        _HeightFade ("smoothness of fade mountain color", Float) = 0.3
    }
    SubShader{
        Pass
        {
            ZWrite on
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma geometry geo
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2g
            {
                float4 vertex : SV_POSITION;
                float4 col : COLOR1;
            };
            struct g2f {
                float4 vertex : SV_POSITION;
                float3 id : COLOR0;
                float4 col : COLOR1;
            };
            
            fixed4 _FloorColor;
            fixed4 _MountainColor;
            float _HeightFade; 
            v2g vert (appdata v)
            {
                v2g o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.col = lerp(_FloorColor, _MountainColor, saturate(v.vertex.y/_HeightFade));
                return o;
            }
            [maxvertexcount(3)]
            void geo (triangle v2g input[3], inout TriangleStream<g2f> tristream){
                g2f o1;
                g2f o2;
                g2f o3;

                o1.vertex = input[0].vertex;
                o2.vertex = input[1].vertex;
                o3.vertex = input[2].vertex;

                o1.id = float3(1, 0, 0);
                o2.id = float3(0, 1, 0);
                o3.id = float3(0, 0, 1);
                o1.col = input[0].col;
                o2.col = input[1].col;
                o3.col = input[2].col;


                tristream.Append(o1);
                tristream.Append(o2);
                tristream.Append(o3);
                /* give each corner of the triangle a unique color.
                somehow this algorithm i use gets the distance to the closest edge using it.*/
            }

            float _FadeDist;
            float _OutlineSize;
            fixed4 _BackgroundColor;
            fixed4 frag (g2f i) : SV_Target
            {
                float scaleOutline = i.vertex.w;
                // make the outline get bigger the further away it is so perspective divide doesn't ruin it.
                fixed dist = 1.-(min(min(i.id.x, i.id.y), i.id.z)/scaleOutline);
                // distance to the closest corner i don't remember where i stole this algorithm from but it works
                dist = smoothstep(0, (1.-_OutlineSize), dist-_OutlineSize);
                // smoothly interpolate between background color and wireframe color
                fixed4 col = lerp(_BackgroundColor, i.col, dist);
                // smoothly interpolate between pixel color and background color based on distance from camera
                return lerp(col, _BackgroundColor, i.vertex.w/_FadeDist);
            }
            ENDCG
        }
    }
}
