//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Ajuna.NetApi.Model.Base;
using Ajuna.NetApi.Model.Types.Base;
using System;
using System.Collections.Generic;


namespace Ajuna.NetApi.Model.Base
{
    
    
    /// <summary>
    /// >> 217 - Array
    /// </summary>
    public sealed class Arr10EnumCell : BaseType
    {
        
        private Ajuna.NetApi.Model.Base.EnumCell[] _value;
        
        public override int TypeSize
        {
            get
            {
                return 10;
            }
        }
        
        public Ajuna.NetApi.Model.Base.EnumCell[] Value
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
            return string.Format("[{0}; {1}]", new Ajuna.NetApi.Model.Base.EnumCell().TypeName(), this.TypeSize);
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
            var array = new Ajuna.NetApi.Model.Base.EnumCell[TypeSize];
            for (var i = 0; i < array.Length; i++) {var t = new Ajuna.NetApi.Model.Base.EnumCell();t.Decode(byteArray, ref p);array[i] = t;};
            var bytesLength = p - start;
            Bytes = new byte[bytesLength];
            Array.Copy(byteArray, start, Bytes, 0, bytesLength);
            Value = array;
        }
        
        public void Create(Ajuna.NetApi.Model.Base.EnumCell[] array)
        {
            Value = array;
            Bytes = Encode();
        }
    }
}
