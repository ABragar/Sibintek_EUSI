using Base;
using CorpProp.Entities.CorporateGovernance;

namespace CorpProp.Entities.ManyToMany
{
    public class EstateAndEstateAppraisal : ManyToManyAssociation<Estate.Estate, EstateAppraisal>
    {
    }
}
