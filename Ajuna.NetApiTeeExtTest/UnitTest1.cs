using Ajuna.NetApi;
using Ajuna.NetApi.Model.AjunaWorker;
using Ajuna.NetApi.Model.PrimitiveTypes;
using Ajuna.NetApi.Model.SpCore;
using Ajuna.NetApi.Model.SpRuntime;
using Ajuna.NetApi.Model.Types;
using Ajuna.NetApi.Model.Types.Base;
using Ajuna.NetApi.Model.Types.Primitive;
using Newtonsoft.Json;
using NUnit.Framework;
using Schnorrkel.Keys;
using SimpleBase;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using TestTee.Model;

namespace Ajuna.NetApiTeeExtTest
{
    public class Tests
    {

        // Secret Key URI `//Alice` is account:
        // Secret seed:      0xe5be9a5092b81bca64be81d212e7f2f9eba183bb7a90954f7b76361f6edb5c0a
        // Public key(hex):  0xd43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d
        // Account ID:       0xd43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d
        // SS58 Address:     5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY
        public static MiniSecret MiniSecretAlice => new MiniSecret(Utils.HexToByteArray("0xe5be9a5092b81bca64be81d212e7f2f9eba183bb7a90954f7b76361f6edb5c0a"), ExpandMode.Ed25519);
        public static Account Alice => Account.Build(KeyType.Sr25519, MiniSecretAlice.ExpandToSecret().ToBytes(), MiniSecretAlice.GetPair().Public.Key);

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TrustedGetterTest()
        {

            var account = new AccountId32();
            account.Create("0xd43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d");

            var trustedGetter = new EnumTrustedGetter();
            trustedGetter.Create(TrustedGetter.Nonce, account);

            Assert.AreEqual("0x02D43593C715FDD31C61141ABD04A99FD6822C8558854CCDE39A5684E7A56DA27D", Utils.Bytes2HexString(trustedGetter.Encode()));
        }

        [Test]
        public void SignatureTest() 
        {

            var account = new AccountId32();
            account.Create("0xd43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d");

            var trustedGetter = new EnumTrustedGetter();
            trustedGetter.Create(TrustedGetter.Nonce, account);

            var signature = new Signature();
            //var signatureArray = Schnorrkel.Sr25519v091.SignSimple(Alice.Bytes, Alice.PrivateKey, trustedGetter.Encode());
            var signatureArray = Schnorrkel.Sr25519v091.SignSimple(MiniSecretAlice.GetPair(), trustedGetter.Encode());
            signature.Create(signatureArray);

            Assert.True(Schnorrkel.Sr25519v091.Verify(signature.Encode(), MiniSecretAlice.GetPair().Public.Key, trustedGetter.Encode()));
            Assert.True(Schnorrkel.Sr25519v091.Verify(Utils.HexToByteArray("0x748A42168264878EDCAD05299251D118750C033F4C8F19D5905C4C4CB08A7B5164667CA74FB69C78B4B86E2001CCC228BE7D447C2A0624A91C3224E581380D8E"), MiniSecretAlice.GetPair().Public.Key, trustedGetter.Encode()));
        }

        [Test]
        public void TrustedOperationTest()
        {
            var account = new AccountId32();
            account.Create("0xd43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d");

            var trustedGetter = new EnumTrustedGetter();
            trustedGetter.Create(TrustedGetter.Nonce, account);

            //var byteArray = JsonConvert.DeserializeObject<byte[]>("[2, 1, 2, 212, 53, 147, 199, 21, 253, 211, 28, 97, 20, 26, 189, 4, 169, 159, 214, 130, 44, 133, 88, 133, 76, 205, 227, 154, 86, 132, 231, 165, 109, 162, 125, 1, 116, 138, 66, 22, 130, 100, 135, 142, 220, 173, 5, 41, 146, 81, 209, 24, 117, 12, 3, 63, 76, 143, 25, 213, 144, 92, 76, 76, 176, 138, 123, 81, 100, 102, 124, 167, 79, 182, 156, 120, 180, 184, 110, 32, 1, 204, 194, 40, 190, 125, 68, 124, 42, 6, 36, 169, 28, 50, 36, 229, 129, 56, 13, 142]");

            var signature = new Signature();
            var signatureArray = Schnorrkel.Sr25519v091.SignSimple(Alice.Bytes, Alice.PrivateKey, trustedGetter.Encode());
            signature.Create(signatureArray);

            var enumMultiSignature = new EnumMultiSignature();
            enumMultiSignature.Create(MultiSignature.Sr25519, signature);

            var trustedGetterSigned = new TrustedGetterSigned();
            trustedGetterSigned.Getter = trustedGetter;
            trustedGetterSigned.Signature = enumMultiSignature;

            var getter = new EnumGetter();
            getter.Create(Getter.Trusted, trustedGetterSigned);

            var trustedOperation = new EnumTrustedOperation();
            trustedOperation.Create(TrustedOperation.Get, getter);
            var firstPart = Utils.Bytes2HexString(trustedOperation.Encode(), Utils.HexStringFormat.Pure).Substring(0, 72);
            var signaPart = Utils.HexToByteArray(Utils.Bytes2HexString(trustedOperation.Encode(), Utils.HexStringFormat.Pure).Substring(72));
            Assert.AreEqual("020102D43593C715FDD31C61141ABD04A99FD6822C8558854CCDE39A5684E7A56DA27D01", firstPart);
            Assert.True(Schnorrkel.Sr25519v091.Verify(signaPart, MiniSecretAlice.GetPair().Public.Key, trustedGetter.Encode()));
            //   
            // 0x
            // 02 - TrustedOperation.Get
            // 01 - Getter.Trusted
            // 02D43593C715FDD31C61141ABD04A99FD6822C8558854CCDE39A5684E7A56DA27D - TrustedGetter
            // --> EnumMultiSignature
            // 01 MultiSignature.Sr25519
            // 748A42168264878EDCAD05299251D118750C033F4C8F19D5905C4C4CB08A7B5164667CA74FB69C78B4B86E2001CCC228BE7D447C2A0624A91C3224E581380D8E - Signature
        }


        //./integritee-cli -U ws://host.docker.internal trusted get-nonce //Alice --mrenclave CAG7CwtvDb5AvC3yoxXetYqY97tGSUdywP1U6pgYf1Kh
        //send trusted getter nonce from 5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY
        //TrustedGetter encoded: 0x02d43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d
        //TrustedGetter signed and wrapped into a TrustedOperation
        //TrustedOperation encoded: 0x020102d43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d012e6aa76d7550c54db5999ea997a028c8e3d40e4b0d3a34ae15c55f949e50940dd9e354fc9e3caf49fa244617cbc486d03cdc61b16cb927e4b03d252ff83c8c8c
        //get_state called
        //encoded operation call: 0x9101020102d43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d012e6aa76d7550c54db5999ea997a028c8e3d40e4b0d3a34ae15c55f949e50940dd9e354fc9e3caf49fa244617cbc486d03cdc61b16cb927e4b03d252ff83c8c8c
        //operation call encrypted: 0x01061225f0574a24b85119fd7dd851cb2465f0a29578e03e2af4bbd2c4530ca139c41cb3d5df59ddf69e9ff23f6e8f083288e62dfb5ae2c3f74d1d13202713b42e63c2423af73256b4b52bf6a4ea07c1b82e0ed075962c0076dc3719714fcb9f5fdba6558dfb94eaa3ece415020f9ca81f8a23a9be0c2fe6cddde77ae304dec3e214df5805d93561686c40ea4dd5344a72e83c7b1dd721ce174d4e872028709c7ff0407f60949dd10e521d17f5f0d0cc9b13e8877f51da6549e25bbfef098f5851123216264586907e79af7d5668d7e5dfa35b31822db92d342eb40f790c70c542b7493c4cee4c7ed17dcec44155a1b29b78850d34e976e352ca45f8174aa9d241d8027a969231489ff61505a2694581ead003a5aca926f50e45d201174c852e85c08ea0b915ccc2a996e009d7e3478a94390e0ef1767069e9a1fd5c464966d87c7cd3427821e947c160b8702ea1bbb1feb714f8426b1a1051988b31b3db51a2466df15b856229e1f0c181483511fb555a592573c7fcff1f5f56495f9bbf7e55b1c7
        //got nonce: 0

        [Test]
        public void EncodedOperationTest()
        {
            var trustedOperation = new EnumTrustedOperation();
            trustedOperation.Create("0x020102D43593C715FDD31C61141ABD04A99FD6822C8558854CCDE39A5684E7A56DA27D01226B0AE8AF1B453AF07452F93B30A479EC7202C3C4ACB29C06861281A9850C7C22A7FC8B73BA8D68327A43BBE4305852077D11AC3E584361B168B3E64BDC388D");
            Assert.AreEqual("0x020102D43593C715FDD31C61141ABD04A99FD6822C8558854CCDE39A5684E7A56DA27D01226B0AE8AF1B453AF07452F93B30A479EC7202C3C4ACB29C06861281A9850C7C22A7FC8B73BA8D68327A43BBE4305852077D11AC3E584361B168B3E64BDC388D", Utils.Bytes2HexString(trustedOperation.Encode()));

            
            var mrenclave = "CAG7CwtvDb5AvC3yoxXetYqY97tGSUdywP1U6pgYf1Kh";

            var shieldingKey = "{\"n\":[229,138,133,93,173,243,195,215,194,205,111,49,66,187,95,125,248,141,38,60,1,202,72,211,75,144,125,255,109,54,6,91,220,1,254,161,108,230,118,7,126,16,104,226,243,124,243,187,87,96,47,184,212,10,232,195,174,59,49,214,126,121,78,154,239,245,85,250,147,52,171,70,39,182,150,170,131,156,13,96,96,107,249,253,32,193,253,32,200,157,47,164,69,21,238,121,194,36,63,217,253,253,70,192,193,42,255,80,42,28,91,163,211,18,57,196,150,3,22,230,176,250,37,176,232,209,125,223,194,104,173,156,78,152,181,70,24,143,169,78,149,1,52,157,192,194,195,253,84,46,38,58,21,25,15,230,40,2,120,155,183,227,50,22,4,64,241,227,219,190,106,106,47,204,7,7,3,152,233,239,168,181,11,46,132,27,189,27,27,133,207,91,98,152,9,116,152,19,225,166,146,92,148,249,9,32,196,85,187,115,140,126,91,30,185,226,64,124,82,165,215,160,237,232,252,65,124,221,6,79,58,242,201,211,254,3,137,245,1,222,248,183,246,198,72,61,35,230,225,19,42,50,107,128,154,255,255,178,232,16,219,134,163,66,82,178,169,233,161,73,165,72,199,9,150,173,86,94,196,27,212,15,16,99,161,188,15,212,71,212,21,83,38,222,185,158,19,246,220,255,162,136,2,160,133,32,115,102,218,191,74,214,32,228,54,179,196,197,116,100,111,45,104,71,31,79,131,89,241,246,76,133,205,249,214,145,114,98,50,62,69,116,104,173,24,224,3,103,177,216,217,117,39,7,190,209,114,216,141,185,2,161,90,195,74,21,159,106,206,135,79,30,185,208,194,43,93,70,126,224,231,33,125,169],\"e\":[1,0,0,1]}";
            var rSAPublicKey = JsonConvert.DeserializeObject<RSAPubKey>(shieldingKey);
            var rSAParameters = new RSAParameters { Modulus = rSAPublicKey.N.ToArray(), Exponent = rSAPublicKey.E.ToArray() };


            var shardId = new H256();
            shardId.Create(Base58.Bitcoin.Decode("CAG7CwtvDb5AvC3yoxXetYqY97tGSUdywP1U6pgYf1Kh").ToArray());
            Assert.AreEqual("0xA5CFDD7152360A2C78859B5596194642720139FDF5CBF621598D2B7B53A763BC", Utils.Bytes2HexString(shardId.Encode()));

            //var cypherText = Utils.RSAEncrypt(trustedOperation.Encode(), new RSAParameters { Modulus = rSAPublicKey.N.ToArray(), Exponent = rSAPublicKey.E.ToArray() }, false);
            //var encryptedExpected = "0xA90AE9DC085F0B42DFD0C5AF0A5D2C683705A9718637ED6BF80EE08AAC9F053BF854ADC93D44B56ED3EED86E68903805CAF7F664FDE99FE043C023D3473482F97A19B85DA5545B3AB4E9D32C089D5A8E8D1C18A9649780611FEBDC32FC96A9923C52C6ABD21D3B13A741A0B875172AEACEF4AD5B10C8108C86C63F2A4CFC8FBE59F55D7FD8B549AD468A1A0193D5554F40D8551DE5E030063A57F73DA44290D2777D07DB2B51E11AD454211730155F8C3AA2865D2C185DBC9052C1861961716FC7E118B86F30D10DBB1A77187AE695DCE7D8C8C113D3D2371891C62DFF1BFFA25BA8C3F38BBFF5405757CFD2808E0B8D10624205EE86902D08B3D9482C99537523F99676C72254A8DC832DE37C660DC23EC7691B78D219C3DA759AFC6B31CC6046FE6FC99768415F8FD3442AF117C2338B78C791953853A19699EB702490C4F26CE2524841C1D3A7385619C7519866DB92C3EE91FE94FFD32E86342284ABA411420211375CE8F4D54A70A7CA8F25915BC9EB1F1082786E7AE095D0620CA97FB7";

            //var bytesExpected = Utils.RSADecrypt(Utils.HexToByteArray(encryptedExpected), rSAParameters, false);
            //var bytesCyphered = Utils.RSADecrypt(cypherText, rSAParameters, false);
            //Assert.AreEqual(Utils.Bytes2HexString(bytesExpected), Utils.Bytes2HexString(bytesCyphered));

            var csp = new RSACryptoServiceProvider(3072);
            
            var encodedBytes = trustedOperation.Encode();
            var encrypted = Utils.RSAEncrypt(trustedOperation.Encode(), csp.ExportParameters(false), null);
            var decrypted = Utils.RSADecrypt(encrypted, csp.ExportParameters(true), null);

            Assert.AreEqual(encodedBytes, decrypted);

            var encryptedWithPub = RSA.Create(rSAParameters);
            var encryptedValue = Utils.RSAEncrypt(trustedOperation.Encode(), encryptedWithPub.ExportParameters(false), null);
            Assert.AreEqual(encryptedValue.Length, encrypted.Length);

            var encryptedTest = Utils.RSAEncrypt(trustedOperation.Encode(), csp.ExportParameters(false), RSAEncryptionPadding.OaepSHA256);
        }

        [Test]
        public void RSAEncryptTest()
        {
            try
            {
                //Create a UnicodeEncoder to convert between byte array and string.
                UnicodeEncoding ByteConverter = new UnicodeEncoding();

                //Create byte arrays to hold original, encrypted, and decrypted data.
                byte[] dataToEncrypt = ByteConverter.GetBytes("Data to Encrypt");
                byte[] encryptedData;
                byte[] decryptedData;

                //Create a new instance of RSACryptoServiceProvider to generate
                //public and private key data.
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {

                    //Pass the data to ENCRYPT, the public key information 
                    //(using RSACryptoServiceProvider.ExportParameters(false),
                    //and a boolean flag specifying no OAEP padding.
                    encryptedData = Utils.RSAEncrypt(dataToEncrypt, RSA.ExportParameters(false), null);

                    //Pass the data to DECRYPT, the private key information 
                    //(using RSACryptoServiceProvider.ExportParameters(true),
                    //and a boolean flag specifying no OAEP padding.
                    decryptedData = Utils.RSADecrypt(encryptedData, RSA.ExportParameters(true), null);

                    Assert.AreEqual(dataToEncrypt, decryptedData);
                }
            }
            catch (ArgumentNullException)
            {
                Assert.Fail();
            }
        }

        private static BaseVec<U8> VecU8FromBytes(byte[] vs)
        {
            var u8list = new List<U8>();
            for (int i = 0; i < vs.Length; i++)
            {
                var u8 = new U8();
                u8.Create(vs[i]);
                u8list.Add(u8);
            }
            var u8Array = u8list.ToArray();

            var result = new BaseVec<U8>();
            result.Create(u8Array);

            return result;
        }
    }
}