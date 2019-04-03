using Base.DAL;
using Base.DAL.EF;
using CorpProp.Entities.Access;

namespace CorpProp.RosReestr
{
    public partial class CorpPropRosReestrConfig
    {

        public static void Init<TContext>(EntityConfigurationBuilder config) where TContext : EFContext
        {
            config.Context(EFRepositoryFactory<TContext>.Instance)
            #region CorpProp.RosReestr.Entities
                        .Entity<CorpProp.RosReestr.Entities.AnotherSubject>()
                        .Entity<CorpProp.RosReestr.Entities.BaseParameter>(en => en
                        .Save(sav => sav
                        .SaveOneObject(s => s.Extract)
                        .SaveOneObject(s => s.ObjectRecord)
                        ))
                        .Entity<CorpProp.RosReestr.Entities.BuildRecord>()
                        .Entity<CorpProp.RosReestr.Entities.CadNumber>()
                        .Entity<CorpProp.RosReestr.Entities.CarParkingSpaceLocationInBuildPlans>()
                        .Entity<CorpProp.RosReestr.Entities.ContourOKSOut>()
                        .Entity<CorpProp.RosReestr.Entities.DealRecord>()
                        .Entity<CorpProp.RosReestr.Entities.DocumentRecord>()
                        .Entity<CorpProp.RosReestr.Entities.ExtractBuild>()
                        .Entity<CorpProp.RosReestr.Entities.ExtractLand>()
                        .Entity<CorpProp.RosReestr.Entities.ExtractNZS>()
                        .Entity<CorpProp.RosReestr.Entities.ExtractObject>()
                        .Entity<CorpProp.RosReestr.Entities.ExtractSubj>()
                        .Entity<CorpProp.RosReestr.Entities.Governance>()
                        .Entity<CorpProp.RosReestr.Entities.IndividualSubject>()
                        .Entity<CorpProp.RosReestr.Entities.LandRecord>()
                        .Entity<CorpProp.RosReestr.Entities.LegalSubject>()
                        .Entity<CorpProp.RosReestr.Entities.NameRecord>()
                        .Entity<CorpProp.RosReestr.Entities.Notice>()
                        .Entity<CorpProp.RosReestr.Entities.NoticeSubj>()
                        .Entity<CorpProp.RosReestr.Entities.ObjectPartNumberRestrictions>()
                        .Entity<CorpProp.RosReestr.Entities.ObjectRecord>()
                        .Entity<CorpProp.RosReestr.Entities.OldNumber>()
                        .Entity<CorpProp.RosReestr.Entities.Organization>()
                        .Entity<CorpProp.RosReestr.Entities.PermittedUse>()
                        .Entity<CorpProp.RosReestr.Entities.PublicSubject>()
                        .Entity<CorpProp.RosReestr.Entities.Refusal>()
                        .Entity<CorpProp.RosReestr.Entities.RefusalSubj>()
                        .Entity<CorpProp.RosReestr.Entities.RestrictedRightsPartyOut>()
                        .Entity<CorpProp.RosReestr.Entities.RestrictRecord>()
                        .Entity<CorpProp.RosReestr.Entities.RightHolder>()
                        .Entity<CorpProp.RosReestr.Entities.RightRecord>()
                        .Entity<CorpProp.RosReestr.Entities.RightRecordNumber>()
                        .Entity<CorpProp.RosReestr.Entities.RoomLocationInBuildPlans>()
                        .Entity<CorpProp.RosReestr.Entities.SubjectRecord>()
                        .Entity<CorpProp.RosReestr.Entities.SubjRight>()
                        .Entity<CorpProp.RosReestr.Entities.TPerson>()

                        .Entity<CorpProp.RosReestr.Migration.MigrateHistory>()
                        .Entity<CorpProp.RosReestr.Migration.MigrateLog>(e => e.Save(s => s.SaveOneObject(o => o.MigrateHistory)
                                                                                          .SaveOneObject(o => o.MigrateState)))
                        .Entity<CorpProp.RosReestr.Migration.MigrateState>()
                        ;
            #endregion
                        
        }
    }
}
