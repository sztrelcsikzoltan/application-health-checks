using InvestmentManager.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace InvestmentManager.DataAccess.EF
{
    public class InvestmentContext : DbContext
    {
        public InvestmentContext(DbContextOptions<InvestmentContext> options) : base(options)
        {

        }

        public DbSet<TradeDate> TradeDates { get; set; } = null!;

        public DbSet<InvestmentAccountType> AccountTypes { get; set; } = null!;

        public DbSet<InvestmentAccount> Accounts { get; set; } = null!;

        public DbSet<AccountMarketValue> AccountMarketValues { get; set; } = null!;

        public DbSet<SecurityType> SecurityTypes { get; set; } = null!;

        public DbSet<Security> Securities { get; set; } = null!;

        public DbSet<SecurityPrice> SecurityPrices { get; set; } = null!;

        public DbSet<AccountPosition> AccountPositions { get; set; } = null!;

        public DbSet<CashFlow> AccountCashFlows { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            this.ConfigureTradeDate(modelBuilder);
            this.ConfigureInvestmentAccountType(modelBuilder);
            this.ConfigureInvestmentAccount(modelBuilder);
            this.ConfigureAccountMarketValue(modelBuilder);
            this.ConfigureSecurityType(modelBuilder);
            this.ConfigureSecurity(modelBuilder);
            this.ConfigureAccountPosition(modelBuilder);
            this.ConfigureCashFlowTypeCode(modelBuilder);
            this.ConfigureCashFlow(modelBuilder);
            this.ConfigureSecurityPrices(modelBuilder);
        }

        private void ConfigureTradeDate(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TradeDate>()
                .ToTable("TradeDates")
                .HasKey(t => t.Date);

            modelBuilder.Entity<TradeDate>().Property(p => p.Date)
                .HasColumnName("TradeDate");
            modelBuilder.Entity<TradeDate>().Property(p => p.IsMonthEnd)
                .HasColumnName("MonthEndDate");
            modelBuilder.Entity<TradeDate>().Property(p => p.IsQuarterEnd)
                .HasColumnName("QuarterEndDate");
            modelBuilder.Entity<TradeDate>().Property(p => p.IsYearEnd)
                .HasColumnName("YearEndDate");
        }

        private void ConfigureInvestmentAccountType(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InvestmentAccountType>()
                .ToTable("AccountTypes")
                .HasKey(t => t.Code);

            modelBuilder.Entity<InvestmentAccountType>().Property(p => p.Code)
                .HasColumnName("AccountTypeCode");
            modelBuilder.Entity<InvestmentAccountType>().Property(p => p.Name)
                .HasColumnName("AccountTypeName");
            modelBuilder.Entity<InvestmentAccountType>().Property(p => p.Prefix)
                .HasColumnName("AccountPrefix");
        }

        private void ConfigureInvestmentAccount(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InvestmentAccount>()
                .ToTable("Accounts")
                .HasKey(a => a.AccountNumber);

            modelBuilder.Entity<InvestmentAccount>()
                .Ignore(a => a.MarketValue);
        }

        private void ConfigureAccountMarketValue(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountMarketValue>()
                .ToTable("AccountMarketValues")
                .HasKey(mv => new { mv.Date, mv.AccountNumber });

            modelBuilder.Entity<AccountMarketValue>().Property(p => p.Date)
                .HasColumnName("TradeDate");

            modelBuilder.Entity<AccountMarketValue>()
                .HasOne<TradeDate>(d => d.TradeDate);
        }

        private void ConfigureSecurityType(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SecurityType>()
                .ToTable("SecurityTypes")
                .HasKey(t => t.Code);

            modelBuilder.Entity<SecurityType>().Property(p => p.Code)
                .HasColumnName("SecurityTypeCode");
            modelBuilder.Entity<SecurityType>().Property(p => p.Name)
                .HasColumnName("SecurityTypeName");
        }

        private void ConfigureSecurity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Security>()
                .ToTable("Securities")
                .HasKey(t => t.Symbol);

            modelBuilder.Entity<Security>().Property(p => p.Symbol)
                .HasColumnName("Ticker");
            modelBuilder.Entity<Security>().Property(p => p.Name)
                .HasColumnName("SecurityName");
        }

        private void ConfigureAccountPosition(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountPosition>()
                .ToTable("AccountPositions")
                .HasKey(p => new { p.Date, p.AccountNumber, p.Symbol });
            modelBuilder.Entity<AccountPosition>().Property(p => p.Date)
                .HasColumnName("TradeDate");
            modelBuilder.Entity<AccountPosition>().Property(p => p.Symbol)
                .HasColumnName("Ticker");
            modelBuilder.Entity<AccountPosition>().Property(p => p.Shares)
                .HasColumnName("Shares");
            modelBuilder.Entity<AccountPosition>().Property(p => p.Price)
                .HasColumnName("Price");
            modelBuilder.Entity<AccountPosition>().Property(p => p.MarketValue)
                .HasColumnName("MarketValue");
            modelBuilder.Entity<AccountPosition>()
                .HasOne<Security>(p => p.Security);
        }


        private void ConfigureCashFlowTypeCode(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CashFlowType>()
                .ToTable("CashFlowTypes")
                .HasKey(cashFlowType => cashFlowType.Code);
            modelBuilder.Entity<CashFlowType>().Property(p => p.Code)
                .HasColumnName("CashFlowTypeCode");
            modelBuilder.Entity<CashFlowType>().Property(p => p.Name)
                .HasColumnName("CashFlowTypeName");
            modelBuilder.Entity<CashFlowType>().Property(p => p.IsExternal)
                .HasColumnName("ExternalFlow");
        }


        private void ConfigureCashFlow(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CashFlow>()
                .ToTable("AccountCashFlows")
                .HasKey(cashFlow => cashFlow.CashFlowId);
            modelBuilder.Entity<CashFlow>().Property(p => p.CashFlowId)
                .HasColumnName("AccountCashFlowId");
            modelBuilder.Entity<CashFlow>().Property(p => p.Date)
                .HasColumnName("TradeDate");
            modelBuilder.Entity<CashFlow>()
                .HasOne<CashFlowType>(x => x.CashFlowType);
        }


        private void ConfigureSecurityPrices(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SecurityPrice>()
                .ToTable("SecurityPrices")
                .HasKey(sp => new { sp.Date, sp.Symbol });

            // using column name "Ticker" instead of the default property name "Symbol"
            modelBuilder.Entity<SecurityPrice>().Property(p => p.Date)
                .HasColumnName("TradeDate");
            // using column name "Ticker" instead of the default property name "Symbol"
            modelBuilder.Entity<SecurityPrice>().Property(p => p.Symbol)
                .HasColumnName("Ticker");

            // setting "TradeDate" as a foreign key
            modelBuilder.Entity<SecurityPrice>()
                .HasOne<TradeDate>(x => x.TradeDate);
            // setting "Security" as a foreign key
            modelBuilder.Entity<SecurityPrice>()
                .HasOne<Security>(x => x.Security);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            optionsBuilder.EnableDetailedErrors(detailedErrorsEnabled: true);
            }
        }
    }
}
