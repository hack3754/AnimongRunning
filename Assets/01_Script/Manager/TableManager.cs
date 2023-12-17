
using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
public class TableManager : MonoBehaviour
{
    RijndaelManaged                             m_Rijndael;
    SHA1CryptoServiceProvider                   m_Provider;

    Dictionary<string, System.IO.StringReader>  m_StringReaders = new Dictionary<string, System.IO.StringReader>();

    public Action<int, int, float>              m_FncLoad;
    int                                         m_Count;

    public void LoadTable( string name, bool isResourceLoad )
    {
        m_Count = 00;
        if ( isResourceLoad )
        {
            LoadTable( ResourceManager.Instance.LoadResource<TextAsset>( "TBL", name ), 0 );
        }
        else
        {
            string key = ResourceManager.Instance.GetKey( ResourceManager.PathType.TBL, name );

            ResourceManager.Instance.LoadAsync<TextAsset>( key, ( assets, isSucess ) =>
            {
                if( isSucess )
                {
                    LoadTable( assets, 0 );
                }
                else
                {
                    LoadTable( ResourceManager.Instance.LoadResource<TextAsset>( "TBL", name ), 0 );
                }
            } );
        }
    }
    public void LoadTables( Action<bool> onEnd )
    {
        ResourceManager.Instance.LoadAsyncs<TextAsset>("txt", LoadTable, ( assets, isSucess ) =>
        {
            onEnd?.Invoke( isSucess );
        } );
    }
    void LoadTable( TextAsset asset, int max )
    {
        if( asset != null )
        {
            if( m_StringReaders.ContainsKey( asset.name ) == false )
            {
#if UNITY_EDITOR
                Debug.Log( $"Load Table : {asset.name}" );
#endif
                m_StringReaders.Add( asset.name, new StringReader( Decrypt( asset.text, asset.name ) ) );
            }
            else
            {
                //!< ReAlloc
                m_StringReaders[ asset.name ] = new StringReader( Decrypt( asset.text, asset.name ) );
            }

            float percent = 0;
            if (max > 0) percent = m_Count / (float)max;

            m_FncLoad?.Invoke(m_Count, max, percent);

            m_Count++;
        }
    }

    public void ReleaseCsv()
    {
        if( m_StringReaders == null ) return;

        foreach( StringReader str in m_StringReaders.Values )
        {
            str.Close();
            str.Dispose();
        }

        m_StringReaders.Clear();
    }

    string Decrypt( string textToDecrypt, string key )
    {
        if( m_Rijndael == null )
        {
            m_Rijndael = new RijndaelManaged();
        }

        byte[] encryptedData = System.Convert.FromBase64String( textToDecrypt );

        byte[] pwdBytes = System.Convert.FromBase64String( GetHeshkey( key ) );

        byte[] newKeysArray = new byte[ 32 ];

        if( pwdBytes.Length < newKeysArray.Length )
            System.Array.Copy( pwdBytes, 0, newKeysArray, 0, pwdBytes.Length );
        else
            System.Array.Copy( pwdBytes, 0, newKeysArray, 0, 32 );

        m_Rijndael.KeySize = 256;
        m_Rijndael.Key = newKeysArray;
        m_Rijndael.Mode = CipherMode.ECB;
        m_Rijndael.Padding = PaddingMode.PKCS7;


        ICryptoTransform icryptoTransform = m_Rijndael.CreateDecryptor();

        byte[] plainText = icryptoTransform.TransformFinalBlock( encryptedData, 0, encryptedData.Length );

        icryptoTransform.Dispose();

        return System.Text.Encoding.UTF8.GetString( plainText );
    }
    string GetHeshkey( string input )
    {
        if( m_Provider == null ) m_Provider = new SHA1CryptoServiceProvider();
        byte[] data = m_Provider.ComputeHash( System.Text.Encoding.UTF8.GetBytes( input ) );
        return System.Convert.ToBase64String( data );
    }
}
