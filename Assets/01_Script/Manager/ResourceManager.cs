using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine.SceneManagement;

public class ResourceManager : MSingleton<ResourceManager>
{
    public enum PathType
    {
        NONE,
        TEXTURE,
        PREFAB,
        SKLETONDATA,
        TBL,
    }

	public bool m_isExcelLoad = true; // 엑셀파일 로드. 에디터용

    Dictionary<string, Texture2D>               m_CharTextrue   = new Dictionary<string, Texture2D>();
    Dictionary<string, AsyncOperationHandle>    m_Handles       = new Dictionary<string, AsyncOperationHandle>();

    private void OnDestroy()
    {
        Release();
		System.GC.Collect();	
    }

    #region 어드레서블

	void AddHandle( string assetName, AsyncOperationHandle handle )
	{
		if (!string.IsNullOrEmpty(assetName) && m_Handles.ContainsKey(assetName))
		{
			Addressables.Release(m_Handles[assetName]);
			m_Handles[assetName] = handle;
		}
		else
		{
			m_Handles.Add(assetName, handle);
		}
	}
	public void Release(string assetName)
	{
		if(!string.IsNullOrEmpty(assetName) && m_Handles.ContainsKey(assetName))
		{
			Addressables.Release(m_Handles[assetName]);
			m_Handles.Remove(assetName);
		}
	}
	public void Release()
	{
		foreach (var handle in m_Handles)
		{
			Addressables.Release(handle.Value);
		}

		m_Handles.Clear();
        m_CharTextrue?.Clear();
    }

    public string GetKey( PathType type, string name )
    {
        string key = name.Split( "." ).First();

        switch( type )
        {
        case PathType.NONE:         return name;
        case PathType.TEXTURE:      return string.Format( $"{key}.png" );
        case PathType.PREFAB:       return string.Format( $"{key}.prefab" );
        case PathType.SKLETONDATA:  return string.Format( $"{key}_SkeletonData.asset" );
        case PathType.TBL:          return string.Format( $"{key}.txt" );
        }

        return null;
    }

    #endregion 어드레서블


    #region CHAR

    readonly string m_TmpTextrueKey = "{0}_{1}{2}{3}{4}";
    readonly int    m_Texture2dSize = 1024;

    public IEnumerator LoadTexture( string name, int hair, int body, int eyeL, int eyeR, Action complete )
    {
        if( string.IsNullOrEmpty( name ) )
            yield break;

        string key = string.Format( m_TmpTextrueKey, name, hair.ToString( "00" ), body.ToString( "00" ), eyeL.ToString( "00" ), eyeR.ToString( "00" ) );

        if( m_CharTextrue.ContainsKey( key ) == false )
        {
            string bodyId = hair.ToString( "00" ) + body.ToString( "00" );
            string eyelId = eyeL.ToString( "00" );
            string eyeRId = eyeR.ToString( "00" );

            var handle = Addressables.LoadAssetsAsync<Texture2D>( name + "Texture", null );

            yield return handle;

            if( handle.Status == AsyncOperationStatus.Succeeded )
            {
                Texture2D tex = new Texture2D( m_Texture2dSize, m_Texture2dSize, TextureFormat.RGBA32, false, true );

                Texture2D texBack = null;
                Texture2D texEyeL = null;
                Texture2D texEyeR = null;
                Texture2D texFront = null;

                for( int i = 0; i < handle.Result.Count; i++ )
                {
                    if( handle.Result[ i ].name.Contains( "Back_" + bodyId ) ) texBack = handle.Result[ i ];
                    else if( handle.Result[ i ].name.Contains( "eye_L_" + eyelId ) ) texEyeL = handle.Result[ i ];
                    else if( handle.Result[ i ].name.Contains( "eye_R_" + eyeRId ) ) texEyeR = handle.Result[ i ];
                    else if( handle.Result[ i ].name.Contains( "Front_" + bodyId ) ) texFront = handle.Result[ i ];

                    if( texBack != null && texEyeL != null && texEyeR != null && texFront != null ) break;
                }

                if( texBack != null && texEyeL != null && texEyeR != null && texFront != null )
                {
                    tex.SetPixels( CombineColor( texBack.GetPixels(), texEyeL.GetPixels(), texEyeR.GetPixels(), texFront.GetPixels() ) );
                    tex.Apply();
                    m_CharTextrue.Add( key, tex );
                }

                texBack = null;
                texEyeL = null;
                texEyeR = null;
                texFront = null;
                //*/
                Addressables.Release( handle );

            }
            else
            {
                Debug.LogError( $"error LoadTexture {key}" );
            }

            complete?.Invoke();

        }
        else
            complete?.Invoke();
    }
    Color[] CombineColor( Color[] back, Color[] eyeL, Color[] eyeR, Color[] front )
    {
        for( int i = 0; i < back.Length; i++ )
        {
            back[ i ] = CombineColor( back[ i ], eyeL[ i ] );
            back[ i ] = CombineColor( back[ i ], eyeR[ i ] );
            back[ i ] = CombineColor( back[ i ], front[ i ] );
        }

        return back;
    }
    Color CombineColor( Color dst, Color src )
    {
        float dstAlpha = dst.a;
        float srcAlpha = src.a;

        float r = 0;
        float g = 0;
        float b = 0;

        if( dstAlpha <= 0 )
        {
            r = src.r * srcAlpha;
            g = src.g * srcAlpha;
            b = src.b * srcAlpha;
        }
        else
        {
            r = ( ( src.r * srcAlpha ) + ( dst.r * ( 1 - srcAlpha ) ) );
            g = ( ( src.g * srcAlpha ) + ( dst.g * ( 1 - srcAlpha ) ) );
            b = ( ( src.b * srcAlpha ) + ( dst.b * ( 1 - srcAlpha ) ) );
        }

        dst = new Color( r, g, b, srcAlpha > dstAlpha ? srcAlpha : dstAlpha );

        return dst;
    }

    #endregion

#if ASYNC
    #region LOAD
    public void Instantiate( string key, Transform perant = null, Action<GameObject> onEnd = null )
    {
        string addressableName = GetKey( PathType.PREFAB, key );

        LoadAsync<GameObject>( addressableName, ( result, isSucess ) =>
        {
            if( isSucess )
            {
                if( result != null )
                {
                    GameObject obj = Instantiate( result, perant );

                    onEnd?.Invoke( obj );

#if UNITY_EDITOR
                    if( UnityEditor.AddressableAssets.AddressableAssetSettingsDefaultObject.Settings.ActivePlayModeDataBuilderIndex != 1 )
                        RefreshMaterial.Refresh( result );
#endif
                }
            }
            else
            {
                Release( addressableName );
                onEnd?.Invoke( null );
            }
        } );
    }

    public T LoadResource<T>( string path, string name ) where T : UnityEngine.Object
    {
        T asset = Resources.Load<T>( $"{path}/{name}" );
        if( null == asset )
        {
            Debug.LogError( $"Resouce Load Failed!! : {path}/{name}" );
            return null;
        }

        return asset;
    }

    public async void LoadAsync<T>( string key, Action<T, bool> onResult ) where T : UnityEngine.Object
    {
        if( m_Handles.ContainsKey( key ) )
        {
            onResult?.Invoke( m_Handles[ key ].Result as T, true );
            return;
        }

        var checkOp = Addressables.LoadResourceLocationsAsync( key );

        await checkOp.Task;

        if( checkOp.Result.Count > 0 )
        {
            if( m_Handles.ContainsKey( key ) )
            {
                onResult?.Invoke( m_Handles[key].Result as T, true );
            }
            else
            {
                var op = Addressables.LoadAssetAsync<T>( key );

                await op.Task;

                if( op.Status == AsyncOperationStatus.Succeeded )
                {
                    if( op.Result != null )
                    {
                        AddHandle( key, op );
                    }
                    else
                        Addressables.Release( op );

                    onResult?.Invoke( op.Task.Result, true );
                }
                else
                {
                    onResult?.Invoke( null, false );
                }
            }
        }
        else
        {
            Debug.LogError( $"Resource Load Failed!! : {key}" );

            onResult?.Invoke( null, false );
        }
    }
    public async void LoadAsyncs<T>( string key, Action<T, int> onProgress, Action<List<T>, bool> onResult ) where T : UnityEngine.Object
    {
        var loadOp = Addressables.LoadResourceLocationsAsync( key );

        await loadOp.Task;

        if( loadOp.Status == AsyncOperationStatus.Succeeded )
        {
            int max = loadOp.Task.Result.Count;

            var handle = Addressables.LoadAssetsAsync<T>( key, ( asset ) => { onProgress?.Invoke( asset, max ); } );

            await handle.Task;

            if( handle.Status == AsyncOperationStatus.Succeeded )
            {
                if( handle.Result != null )
                    AddHandle( key, handle );
                else
                    Addressables.Release( handle );

                onResult?.Invoke( handle.Task.Result.ToList(), true );
            }
        }
    }

    public async void LoadScene( JMSceneLoadManager.SCENES scenes, Action<float> progress, Action onFinish, LoadSceneMode loadMode = LoadSceneMode.Single )
    {
        string scenesName = string.Empty;
        switch( scenes )
        {
        case JMSceneLoadManager.SCENES.Lobby:
            scenesName = "LobbyScene.unity";
            break;
        case JMSceneLoadManager.SCENES.TestLobbyScene:
            scenesName = "TestLobbyScene.unity";
            break;
        case JMSceneLoadManager.SCENES.Game:
            scenesName = "GameScene.unity";
            break;
        case JMSceneLoadManager.SCENES.Chapter:
            scenesName = "ChapterScene.unity";
            break;
        }

        // 로그인씬은 일반로딩
        if( scenes == JMSceneLoadManager.SCENES.Login )
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene( "LoginScene" );
        }
        else
        {
            var operation = Addressables.LoadSceneAsync( scenesName, loadMode );
            operation.Completed += ( handle ) =>
            {
                if( handle.Status == AsyncOperationStatus.Succeeded )
                {
                    AsyncOperation op = handle.Result.ActivateAsync();
                    op.completed += ( op ) =>
                    {
#if UNITY_EDITOR
                        foreach( GameObject obj in operation.Result.Scene.GetRootGameObjects() )
                            RefreshMaterial.Refresh( obj );
#endif

                        onFinish?.Invoke();
                    };
                }
                else
                {
                    Debug.LogError( "Scene Load Failed!!" );
                }
            };

            while( !operation.IsDone )
            {
                progress?.Invoke( operation.PercentComplete );
                await Task.Yield();
            }
        }
    }

    #endregion
#else
    #region LOAD
    public void Instantiate( string key, Transform perant = null, Action<GameObject> onEnd = null )
    {
        string addressableName = key;//GetKey( PathType.PREFAB, key );
        
        LoadAsync<GameObject>( addressableName, ( result, isSucess ) =>
        {
            if( isSucess )
            {
                if( result != null )
                {
                    GameObject obj = Instantiate( result, perant );

                    onEnd?.Invoke( obj );

#if UNITY_EDITOR
                    if( UnityEditor.AddressableAssets.AddressableAssetSettingsDefaultObject.Settings.ActivePlayModeDataBuilderIndex != 1 ) RefreshMaterial.Refresh( result );
#endif
                }
            }
            else
            {
                onEnd?.Invoke( null );
            }
        } );
    }

    public IEnumerator CoInstantiate(string key, Transform perant = null, Action<GameObject> onEnd = null)
    {
        var checkOp = Addressables.LoadResourceLocationsAsync(key);

        yield return checkOp;
        if (checkOp.Result.Count > 0)
        {
            if (m_Handles.ContainsKey(key))
            {
                onEnd?.Invoke(m_Handles[key].Result as GameObject);
            }
            else
            {
                var op = Addressables.InstantiateAsync(key, perant);

                yield return op;

                if (op.Status == AsyncOperationStatus.Succeeded)
                {
                    if (op.Result != null)
                    {
                        //AddHandle(key, op);
                    }

                    onEnd?.Invoke(op.Task.Result);
                }
                else
                {
                    onEnd?.Invoke(null);
                }
            }
        }
        else
        {
            Debug.LogError($"Resource Load Failed!! : {key}");

            onEnd?.Invoke(null);
        }
    }

    public T LoadResource<T>( string path, string name ) where T : UnityEngine.Object
    {
        T asset = Resources.Load<T>( $"{path}/{name}" );
        if( null == asset )
        {
            Debug.LogError( $"Resouce Load Failed!! : {path}/{name}" );
            return null;
        }

        return asset;
    }

    public void LoadAsync<T>( string key, Action<T, bool> onResult ) where T : UnityEngine.Object
    {
        if( m_Handles.ContainsKey( key ) )
        {
            onResult?.Invoke( m_Handles[ key ].Result as T, true );
            return;
        }

        StartCoroutine( LoadAsset() );

        IEnumerator LoadAsset()
        {
            var checkOp = Addressables.LoadResourceLocationsAsync( key );

            yield return checkOp;

            if( checkOp.Result.Count > 0 )
            {
                if( m_Handles.ContainsKey( key ) )
                {
                    onResult?.Invoke( m_Handles[ key ].Result as T, true );
                }
                else
                {
                    var op = Addressables.LoadAssetAsync<T>( key );

                    yield return op;

                    if( op.Status == AsyncOperationStatus.Succeeded )
                    {
                        if( op.Result != null )
                        {
                            AddHandle( key, op );
                        }
                        else
                            Addressables.Release( op );

                        onResult?.Invoke( op.Task.Result, true );
                    }
                    else
                    {
                        onResult?.Invoke( null, false );
                    }
                }
            }
            else
            {
                Debug.LogError( $"Resource Load Failed!! : {key}" );

                onResult?.Invoke( null, false );
            }
        }
    }
    public void LoadAsyncs<T>( string key, Action<T, int> onProgress, Action<List<T>, bool> onResult ) where T : UnityEngine.Object
    {
        StartCoroutine( LoadAssets() );

        IEnumerator LoadAssets()
        {
            var loadOp = Addressables.LoadResourceLocationsAsync( key );

            yield return loadOp;

            if( loadOp.Status == AsyncOperationStatus.Succeeded )
            {
                int max = loadOp.Task.Result.Count;

                var handle = Addressables.LoadAssetsAsync<T>( key, ( asset ) => { onProgress?.Invoke( asset, max ); } );

                yield return handle;

                if( handle.Status == AsyncOperationStatus.Succeeded )
                {
                    if( handle.Result != null )
                        AddHandle( key, handle );
                    else
                        Addressables.Release( handle );

                    onResult?.Invoke( handle.Task.Result.ToList(), true );
                }
            }
        }
    }
    /*
    public void LoadScene( JMSceneLoadManager.SCENES scenes, Action<float> progress, Action onFinish, LoadSceneMode loadMode = LoadSceneMode.Single )
    {
        string scenesName = string.Empty;
        switch( scenes )
        {
        case JMSceneLoadManager.SCENES.Lobby:
            scenesName = "LobbyScene.unity";
            break;
        case JMSceneLoadManager.SCENES.TestLobbyScene:
            scenesName = "TestLobbyScene.unity";
            break;
        case JMSceneLoadManager.SCENES.Game:
            scenesName = "GameScene.unity";
            break;
        case JMSceneLoadManager.SCENES.Chapter:
            scenesName = "ChapterScene.unity";
            break;
        }

        StartCoroutine( LoadScene() );

        IEnumerator LoadScene()
        {
            // 로그인씬은 일반로딩
            if( scenes == JMSceneLoadManager.SCENES.Login )
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene( "LoginScene" );
            }
            else
            {
                var operation = Addressables.LoadSceneAsync( scenesName, loadMode );
                operation.Completed += ( handle ) =>
                {
                    if( handle.Status == AsyncOperationStatus.Succeeded )
                    {
                        AsyncOperation op = handle.Result.ActivateAsync();
                        op.completed += ( op ) =>
                        {
#if UNITY_EDITOR
                            
                            foreach( GameObject obj in operation.Result.Scene.GetRootGameObjects() )
                                RefreshMaterial.Refresh( obj );
                            
#endif

                            onFinish?.Invoke();
                        };
                    }
                    else
                    {
                        Debug.LogError( "Scene Load Failed!!" );
                    }
                };

                while( !operation.IsDone )
                {
                    progress?.Invoke( operation.PercentComplete );
                    yield return new WaitForEndOfFrame();
                }
            }
        }
    }
    */
    #endregion
#endif
}
