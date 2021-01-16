 Shader "Custom/FogOfWar" {
     Properties {
         _Color("Main Color", Color) = (1,1,1,1)
         _MainTex("Base (RGB)", 2D) = "white" {}
     }
 
     SubShader {
         Tags {"RenderType"="Transparent" "LightMode"="ForwardBase"}
         //Blend SrcAlpha OneMinusSrcAlpha
         Lighting Off
         LOD 200
 
         CGPROGRAM
         #pragma surface surf NoLighting noambient alpha
 
         fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, float aten)
         {
                 fixed4 color;
                 color.rgb = s.Albedo;
                 color.a = s.Alpha;
                 return color;
         }
         
         sampler2D _MainTex;
         fixed4     _Color;
 
         struct Input {
             float2 uv_MainTex;
         };
 
         void surf(Input IN, inout SurfaceOutput o) {
             half4 baseColor = tex2D(_MainTex, IN.uv_MainTex);
             o.Albedo = _Color.rgb * baseColor.b;
             o.Alpha = _Color.a - baseColor.g; // Green - colour of aperture mask
         }
         ENDCG
     }
 }