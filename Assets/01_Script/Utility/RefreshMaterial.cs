using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Diagnostics;

public static class RefreshMaterial
{
	[Conditional( "UNITY_EDITOR" )]
	static public void Refresh( Material mat )
	{
		if( null != mat && null != mat.shader )
		{
			mat.shader = Shader.Find( mat.shader.name.ToString() );
		}
	}

	[Conditional( "UNITY_EDITOR" )]
	static public void Refresh( GameObject go )
	{
		if( null == go )
			return;

		try
		{
			var gList = go.GetComponentsInChildren<Graphic>( true );
			if( gList != null && 0 < gList.Length )
			{
				foreach( var it in gList )
				{
					//!< Text
					var text = it.GetComponent<TMPro.TextMeshProUGUI>();
					if (null != text)
					{
						Refresh(text.materialForRendering);
					}
					else
                    {
						if (it.material != null)
							it.material.shader = Shader.Find(it.material.shader.name);
					}
				}
			}

			var rList = go.GetComponentsInChildren<Renderer>( true );
			if( rList != null && 0 < rList.Length )
			{
				foreach( var it in rList )
				{
					foreach (var mtrl in it.materials)
					{
						if (mtrl != null)
							mtrl.shader = Shader.Find(mtrl.shader.name);
					}
				}
			}
			var pList = go.GetComponentsInChildren<Projector>( true );
			if( pList != null && 0 < pList.Length )
			{
				foreach( var it in pList )
				{
					if( it.material != null )
						it.material.shader = Shader.Find( it.material.shader.name );
				}
			}
		}
		catch( System.Exception ex )
		{
			UnityEngine.Debug.LogError( ex.StackTrace );
		}
	}
}
