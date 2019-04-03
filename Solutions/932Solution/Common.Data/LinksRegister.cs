using Base.Links.Entities;

namespace Data
{
    internal static class LinksRegister
    {
        public static void Reg(ILinkBuilder linkBuilder)
        {
            LinksRegisterTest.Reg(linkBuilder);
            LinksRegisterContact.Reg(linkBuilder);
        }
    }
}