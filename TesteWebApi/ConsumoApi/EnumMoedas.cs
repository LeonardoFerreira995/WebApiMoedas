using System;
using System.ComponentModel;

namespace ConsumoApi
{
    public class EnumMoedas
    {
        public static int PegarEnumPorDescricao(string descricao, Type enumTipo)
        {
            foreach (var field in enumTipo.GetFields())
            {
                var atributo = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))
                                                                        as DescriptionAttribute;
                if (atributo is null)
                    continue;
                if (atributo.Description == descricao)
                {
                    return (int)field.GetValue(null);
                }
            }
            return 0;
        }
    }
}
