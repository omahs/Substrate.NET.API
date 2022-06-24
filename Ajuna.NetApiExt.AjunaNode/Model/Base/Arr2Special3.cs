//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Ajuna.NetApi.Model.SpCore;
using Ajuna.NetApi.Model.Types.Base;
using Ajuna.NetApi.Model.Types.Primitive;
using System;
using System.Collections.Generic;


namespace Ajuna.NetApi.Model.Base
{
    
    
    /// <summary>
    /// >> 223 - Array
    /// </summary>
    public sealed class Arr2Special3 : BaseType
    {
        
        private BaseTuple<Ajuna.NetApi.Model.SpCore.AccountId32,Ajuna.NetApi.Model.Types.Primitive.U8>[] _value;
        
        public override int TypeSize
        {
            get
            {
                return 2;
            }
        }
        
        public BaseTuple<Ajuna.NetApi.Model.SpCore.AccountId32,Ajuna.NetApi.Model.Types.Primitive.U8>[] Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
            }
        }
        
        public override string TypeName()
        {
            return string.Format("[{0}; {1}]", new BaseTuple<Ajuna.NetApi.Model.SpCore.AccountId32,Ajuna.NetApi.Model.Types.Primitive.U8>().TypeName(), this.TypeSize);
        }
        
        public override byte[] Encode()
        {
            var result = new List<byte>();
            foreach (var v in Value){result.AddRange(v.Encode());};
            return result.ToArray();
        }
        
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            var array = new BaseTuple<Ajuna.NetApi.Model.SpCore.AccountId32,Ajuna.NetApi.Model.Types.Primitive.U8>[TypeSize];
            for (var i = 0; i < array.Length; i++) {var t = new BaseTuple<Ajuna.NetApi.Model.SpCore.AccountId32,Ajuna.NetApi.Model.Types.Primitive.U8>();t.Decode(byteArray, ref p);array[i] = t;};
            var bytesLength = p - start;
            Bytes = new byte[bytesLength];
            Array.Copy(byteArray, start, Bytes, 0, bytesLength);
            Value = array;
        }
        
        public void Create(BaseTuple<Ajuna.NetApi.Model.SpCore.AccountId32,Ajuna.NetApi.Model.Types.Primitive.U8>[] array)
        {
            Value = array;
            Bytes = Encode();
        }
    }
}