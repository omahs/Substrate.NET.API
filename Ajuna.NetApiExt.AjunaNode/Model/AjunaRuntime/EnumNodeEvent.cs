//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Ajuna.NetApi.Model.Types.Base;
using System;
using System.Collections.Generic;


namespace Ajuna.NetApi.Model.AjunaRuntime
{
    
    
    public enum NodeEvent
    {
        
        System,
        
        Grandpa,
        
        Balances,
        
        Assets,
        
        Sudo,
        
        Teerex,
        
        ConnectFour,
        
        Scheduler,
        
        Matchmaker,
        
        GameRegistry,
        
        Observers,
    }
    
    /// <summary>
    /// >> 17 - Variant[ajuna_runtime.Event]
    /// </summary>
    public sealed class EnumNodeEvent : BaseEnum<NodeEvent>
    {
    }
}