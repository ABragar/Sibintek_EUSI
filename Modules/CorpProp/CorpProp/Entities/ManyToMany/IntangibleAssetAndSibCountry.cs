using Base;
using CorpProp.Entities.Estate;
using CorpProp.Entities.FIAS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpProp.Entities.ManyToMany
{
    
    public class IntangibleAssetAndSibCountry : ManyToManyAssociation<IntangibleAsset, SibCountry>
    {
    }
}
