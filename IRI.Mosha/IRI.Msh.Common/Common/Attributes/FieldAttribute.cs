using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Msh.Common.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class FieldAttribute : Attribute
{
    public string Alias { get; set; }

	//public FieldAttribute(string alias)
	//{
	//	this.Alias = alias;
	//}
}
