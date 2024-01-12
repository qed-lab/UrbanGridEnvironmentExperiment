// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader"QEDLab/Covert"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _CenterPosition("CenterPosition", Vector) = (0,0,0,0)
        _Radius("Radius", float) = 1
        _Modulation("Modulation", float) = 1
    }
    SubShader
    {
        Pass
        {
            Tags {"LightMode"="ForwardBase"}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            
            // compile shader into multiple variants, with and without shadows
            // (we don't care about any lightmaps yet, so skip these variants)
            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
            // shadow helper functions and macros
            #include "AutoLight.cginc"
            
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                SHADOW_COORDS(1) // put shadows data into TEXCOORD1
                fixed3 diff : COLOR0;
                fixed3 ambient : COLOR1;
                float4 pos : SV_POSITION;
            };
            v2f vert(appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.uv = v.texcoord;
                half3 worldNormal = UnityObjectToWorldNormal(v.normal);
                half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
                o.diff = nl * _LightColor0.rgb;
                o.ambient = ShadeSH9(half4(worldNormal, 1));
                // compute shadows data
                TRANSFER_SHADOW(o)
            
                return o;
            }

            sampler2D _MainTex;
            fixed4 _CenterPosition;
            float _Radius;
            float _Modulation;
            float3 tmp;

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                // compute shadow attenuation (1.0 = fully lit, 0.0 = fully shadowed)
                fixed shadow = SHADOW_ATTENUATION(i);
                // darken light's illumination with shadow, keep ambient intact
                fixed3 lighting = i.diff * shadow + i.ambient;
                col.rgb *= lighting;
    
                //---Covert
                //Get the vector between this pixel and the center of the object
                tmp = i.worldPos.xyz - _CenterPosition.xyz;
                //Convert the vector into the length (the distance)
                float tmpDis = ((_Radius - length(tmp)) / _Radius);
                //Limmit the length between 0~1
                if (tmpDis > 1)
                {
                    tmpDis = 1;
                }
                else if (tmpDis < 0)
                {
                    tmpDis = 0;
                }
                //Weight the length and multiply it with (1,1,1) white
                col.rgb += 0.095f * tmpDis * _Modulation * (1, 1, 1);
                //---Covert
                return col;
            }
            ENDCG
        }
        
        // shadow casting support
        UsePass"Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}