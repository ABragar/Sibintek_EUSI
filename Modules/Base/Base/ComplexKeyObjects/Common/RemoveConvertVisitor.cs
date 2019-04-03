using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Base.ComplexKeyObjects.Common
{
    public class RemoveConvertVisitor : ExpressionVisitor
    {
        internal static readonly RemoveConvertVisitor Instance = new RemoveConvertVisitor();
        protected override Expression VisitMember(MemberExpression node)
        {

            if (node.Member.DeclaringType.IsInterface)
            {
                var expression = node.Expression;
                if (!expression.Type.IsInterface)
                {

               
                    var property = FindPropertyInfo(expression.Type, (PropertyInfo)node.Member);

                    if (property != null)
                        return Expression.Property(Visit(expression), property);
                }
                else
                if (expression.NodeType == ExpressionType.Convert)
                {
                    var convert = (UnaryExpression)expression;

                    var property = FindPropertyInfo(convert.Operand.Type, (PropertyInfo)node.Member);

                    if (property != null)
                        return Expression.Property(Visit(convert.Operand), property);
                }

            }
            return base.VisitMember(node);
        }

        protected PropertyInfo FindPropertyInfo(Type type, PropertyInfo base_property_info)
        {
            return type.GetProperty(base_property_info.Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        }

    }
}