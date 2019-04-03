﻿using Base.DAL;
using Base.Service;

namespace Base.Security.Service
{
    public interface IBaseUserCategoryService<T> : IBaseCategoryService<T> where T : UserCategory
    {
    }
}