using Base;
using CorpProp.Entities.Subject;

namespace CorpProp.Entities.ManyToMany
{
    public class SocietyAgents : ManyToManyAssociation<Society, Society>
    {
        public SocietyAgents()
        {

        }

    }
}
