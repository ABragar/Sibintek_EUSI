using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Links.Entities;
using Common.Data.Entities.Test;

namespace Data
{
    public static class LinksRegisterTest
    {
        public static void Reg(ILinkBuilder linkBuilder)
        {
            linkBuilder.Register<TestObject, TestObject>().Config((source, dest) =>
            {
                dest.Bullshit = !source.Bullshit;
                dest.Title = source.Title + "New object title";
                //dest.Date = source.Date?.AddDays(2);
                //dest.DateTest = source.DateTest;
                //dest.DateTimeTest = source.DateTimeTest;
                dest.MonthTest = source.MonthTest;
            });
        }
    }
}