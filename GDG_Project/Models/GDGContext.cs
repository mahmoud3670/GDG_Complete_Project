using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GDG_Project.Models
{
    public partial class GDGContext : DbContext
    {
        public GDGContext()
        {
        }

        public GDGContext(DbContextOptions<GDGContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Activates> Activates { get; set; }
        public virtual DbSet<ContactUs> ContactUs { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<EmpSalary> EmpSalary { get; set; }
        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<LogEvent> LogEvent { get; set; }
        public virtual DbSet<Member> Member { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Payment> Payment { get; set; }
        public virtual DbSet<PersonInfo> PersonInfo { get; set; }
        public virtual DbSet<School> School { get; set; }
        public virtual DbSet<TimeMachine> TimeMachine { get; set; }
        public virtual DbSet<Tournaments> Tournaments { get; set; }
        public virtual DbSet<Trainer> Trainer { get; set; }
        public virtual DbSet<TrainerSalary> TrainerSalary { get; set; }

      

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<Activates>(entity =>
            {
                entity.HasKey(e => e.ActId)
                    .HasName("PK_Activates_Act_ID");

                entity.HasIndex(e => e.ActName)
                    .HasName("UN_Activates_Act_Name")
                    .IsUnique();

                entity.Property(e => e.ActId).HasColumnName("Act_ID");

                entity.Property(e => e.ActActive).HasColumnName("Act_Active");

                entity.Property(e => e.ActDescription)
                    .IsRequired()
                    .HasColumnName("Act_Description")
                    .HasColumnType("ntext");

                entity.Property(e => e.ActImg)
                    .HasColumnName("Act_Img")
                    .HasMaxLength(250);

                entity.Property(e => e.ActMinAge).HasColumnName("Act_MinAge");

                entity.Property(e => e.ActName)
                    .IsRequired()
                    .HasColumnName("Act_Name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ContactUs>(entity =>
            {
                entity.HasKey(e => e.MessageId);

                entity.Property(e => e.MessageId).HasColumnName("Message_ID");

                entity.Property(e => e.AnonymsEmail)
                    .HasColumnName("Anonyms_Email")
                    .HasMaxLength(50);

                entity.Property(e => e.AnonymsMessage)
                    .IsRequired()
                    .HasColumnName("Anonyms_Message")
                    .HasColumnType("ntext");

                entity.Property(e => e.AnonymsName)
                    .IsRequired()
                    .HasColumnName("Anonyms_Name")
                    .HasMaxLength(50);

                entity.Property(e => e.AnonymsPhone)
                    .IsRequired()
                    .HasColumnName("Anonyms_Phone")
                    .HasMaxLength(50);

                entity.Property(e => e.MessageDate)
                    .HasColumnName("Message_Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Opend).HasColumnName("opend");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(e => e.DepId)
                    .HasName("PK_Department_Dep_ID");

                entity.Property(e => e.DepId).HasColumnName("Dep_ID");

                entity.Property(e => e.DepName)
                    .IsRequired()
                    .HasColumnName("Dep_Name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<EmpSalary>(entity =>
            {
                entity.HasKey(e => e.SalaryId)
                    .HasName("PK_Emp_Salary");

                entity.Property(e => e.SalaryId).HasColumnName("Salary_ID");

                entity.Property(e => e.EmpId).HasColumnName("Emp_ID");

                entity.Property(e => e.SalaryDate)
                    .HasColumnName("Salary_Date")
                    .HasColumnType("date");

                entity.HasOne(d => d.Emp)
                    .WithMany(p => p.EmpSalaryNavigation)
                    .HasForeignKey(d => d.EmpId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_Salary_Emp");
            });

            modelBuilder.Entity<Employees>(entity =>
            {
                entity.HasKey(e => e.EmpId)
                    .HasName("PK_Employees_Emp_ID");

                entity.HasIndex(e => e.EmpUserName)
                    .HasName("UN_Employees_Emp_UserName")
                    .IsUnique();

                entity.Property(e => e.EmpId).HasColumnName("Emp_ID");

                entity.Property(e => e.EmpActive)
                    .HasColumnName("Emp_Active")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.EmpDepartment).HasColumnName("Emp_Department");

                entity.Property(e => e.EmpInfo).HasColumnName("Emp_Info");

                entity.Property(e => e.EmpPassword)
                    .HasColumnName("Emp_Password")
                    .HasMaxLength(50);

                entity.Property(e => e.EmpPostion)
                    .HasColumnName("Emp_Postion")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.EmpSalary)
                    .HasColumnName("Emp_Salary")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.EmpUserName)
                    .HasColumnName("Emp_UserName")
                    .HasMaxLength(50);

                entity.HasOne(d => d.EmpDepartmentNavigation)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.EmpDepartment)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Employees_Emp_Department");

                entity.HasOne(d => d.EmpInfoNavigation)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.EmpInfo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Employees_Emp_ID");
            });

            modelBuilder.Entity<LogEvent>(entity =>
            {
                entity.HasKey(e => e.LogId);

                entity.Property(e => e.LogId).HasColumnName("LogID");

                entity.Property(e => e.EventName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.EventReport)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.HasOne(d => d.EventActorNavigation)
                    .WithMany(p => p.LogEvent)
                    .HasForeignKey(d => d.EventActor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LogEvent_EventActor");
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.Property(e => e.ActInfo).HasColumnName("Act_Info");

                entity.Property(e => e.MemberActive).HasColumnName("Member_Active");

                entity.Property(e => e.MemberInfo).HasColumnName("Member_Info");

                entity.Property(e => e.StartDate)
                    .HasColumnName("Start_Date")
                    .HasColumnType("date");

                entity.HasOne(d => d.ActInfoNavigation)
                    .WithMany(p => p.Member)
                    .HasForeignKey(d => d.ActInfo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Member_ToActivty");

                entity.HasOne(d => d.MemberInfoNavigation)
                    .WithMany(p => p.Member)
                    .HasForeignKey(d => d.MemberInfo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Member_PersonInfo");
            });

            modelBuilder.Entity<News>(entity =>
            {
                entity.Property(e => e.NewsId).HasColumnName("News_ID");

                entity.Property(e => e.NewsContent)
                    .IsRequired()
                    .HasColumnName("News_Content")
                    .HasColumnType("ntext");

                entity.Property(e => e.NewsDate)
                    .HasColumnName("News_Date")
                    .HasColumnType("datetime");

                entity.Property(e => e.NewsImg).HasColumnName("News_Img");

                entity.Property(e => e.NewsNviwer).HasColumnName("News_NViwer");

                entity.Property(e => e.NewsTitle)
                    .IsRequired()
                    .HasColumnName("News_Title")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.PayId)
                    .HasName("PK_Pay_ID");

                entity.Property(e => e.PayId).HasColumnName("Pay_ID");

                entity.Property(e => e.MemberInfo).HasColumnName("Member_Info");

                entity.Property(e => e.PayAmountPaid).HasColumnName("Pay_AmountPaid");

                entity.Property(e => e.PayDate)
                    .HasColumnName("Pay_Date")
                    .HasColumnType("date");

                entity.Property(e => e.PayDiscount)
                    .HasColumnName("Pay_Discount")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.PayDueDate)
                    .HasColumnName("Pay_DueDate")
                    .HasColumnType("date");

                entity.Property(e => e.PayType)
                    .HasColumnName("Pay_Type")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.SchoolId).HasColumnName("School_ID");

                entity.Property(e => e.TrainerCode).HasColumnName("Trainer_Code");

                entity.HasOne(d => d.MemberInfoNavigation)
                    .WithMany(p => p.Payment)
                    .HasForeignKey(d => d.MemberInfo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Payment_Member_Code");

                entity.HasOne(d => d.School)
                    .WithMany(p => p.Payment)
                    .HasForeignKey(d => d.SchoolId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Payment_School_ID");

                entity.HasOne(d => d.TrainerCodeNavigation)
                    .WithMany(p => p.Payment)
                    .HasForeignKey(d => d.TrainerCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Payment_Trainer_Code");
            });

            modelBuilder.Entity<PersonInfo>(entity =>
            {
                entity.HasKey(e => e.PId)
                    .HasName("PK_PersonInfo_P_ID");

                entity.HasIndex(e => e.PNationalId)
                    .HasName("UN_PersonInfo_P_National_ID")
                    .IsUnique();

                entity.Property(e => e.PId).HasColumnName("P_ID");

                entity.Property(e => e.PAdress)
                    .IsRequired()
                    .HasColumnName("P_Adress")
                    .HasMaxLength(50);

                entity.Property(e => e.PBirthDate)
                    .HasColumnName("P_BirthDate")
                    .HasColumnType("date");

                entity.Property(e => e.PEmail)
                    .HasColumnName("p_Email")
                    .HasMaxLength(100);

                entity.Property(e => e.PGender)
                    .IsRequired()
                    .HasColumnName("P_Gender")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.PImg)
                    .HasColumnName("P_Img")
                    .HasMaxLength(250);

                entity.Property(e => e.PName)
                    .IsRequired()
                    .HasColumnName("P_Name")
                    .HasMaxLength(50);

                entity.Property(e => e.PNationalId)
                    .IsRequired()
                    .HasColumnName("P_National_ID")
                    .HasMaxLength(15);

                entity.Property(e => e.PPhone)
                    .IsRequired()
                    .HasColumnName("P_Phone")
                    .HasMaxLength(15);

                entity.Property(e => e.PStartDate)
                    .HasColumnName("P_StartDate")
                    .HasColumnType("date");

                entity.Property(e => e.PType)
                    .IsRequired()
                    .HasColumnName("P_Type")
                    .HasMaxLength(1)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<School>(entity =>
            {
                entity.Property(e => e.SchoolId).HasColumnName("School_ID");

                entity.Property(e => e.SchoolAct).HasColumnName("School_Act");

                entity.Property(e => e.SchoolActive).HasColumnName("School_Active");

                entity.Property(e => e.SchoolDays)
                    .IsRequired()
                    .HasColumnName("School_Days")
                    .HasMaxLength(50);

                entity.Property(e => e.SchoolEndDay)
                    .HasColumnName("School_EndDay")
                    .HasColumnType("date");

                entity.Property(e => e.SchoolLevel)
                    .IsRequired()
                    .HasColumnName("School_Level")
                    .HasMaxLength(50);

                entity.Property(e => e.SchoolMaxMember).HasColumnName("School_MaxMember");

                entity.Property(e => e.SchoolMaxTrainer).HasColumnName("School_MaxTrainer");

                entity.Property(e => e.SchoolMemberCount)
                    .HasColumnName("School_Member_Count")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.SchoolName)
                    .IsRequired()
                    .HasColumnName("School_Name")
                    .HasMaxLength(50);

                entity.Property(e => e.SchoolPrice)
                    .HasColumnName("School_Price")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.SchoolStartDay)
                    .HasColumnName("School_StartDay")
                    .HasColumnType("date");

                entity.Property(e => e.SchoolTrainerCount)
                    .HasColumnName("School_Trainer_Count")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.TrainerPercent).HasColumnName("Trainer_Percent");

                entity.HasOne(d => d.SchoolActNavigation)
                    .WithMany(p => p.School)
                    .HasForeignKey(d => d.SchoolAct)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_School_Act");
            });

            modelBuilder.Entity<TimeMachine>(entity =>
            {
                entity.HasKey(e => e.TimeId);

                entity.Property(e => e.TimeId).HasColumnName("Time_ID");

                entity.Property(e => e.EmpId).HasColumnName("Emp_ID");

                entity.Property(e => e.NoteCase)
                    .HasColumnName("Note_Case")
                    .HasMaxLength(20);

                entity.Property(e => e.TimeDate)
                    .HasColumnName("Time_Date")
                    .HasColumnType("date");

                entity.Property(e => e.TimeIn).HasColumnName("Time_In");

                entity.Property(e => e.TimeOut).HasColumnName("Time_Out");

                entity.HasOne(d => d.Emp)
                    .WithMany(p => p.TimeMachine)
                    .HasForeignKey(d => d.EmpId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TimeMachine_Emp_ID");
            });

            modelBuilder.Entity<Tournaments>(entity =>
            {
                entity.HasKey(e => e.TourId)
                    .HasName("PK_Tournaments_ID");

                entity.Property(e => e.TourId).HasColumnName("Tour_ID");

                entity.Property(e => e.ActId).HasColumnName("Act_ID");

                entity.Property(e => e.MemberImg)
                    .IsRequired()
                    .HasColumnName("Member_img")
                    .HasMaxLength(250);

                entity.Property(e => e.MemberInfo).HasColumnName("Member_Info");

                entity.Property(e => e.MemberLevel)
                    .IsRequired()
                    .HasColumnName("Member_Level")
                    .HasMaxLength(50);

                entity.Property(e => e.TourDate)
                    .HasColumnName("Tour_Date")
                    .HasColumnType("date");

                entity.Property(e => e.TourDescription)
                    .IsRequired()
                    .HasColumnName("Tour_Description")
                    .HasMaxLength(250);

                entity.Property(e => e.TourName)
                    .IsRequired()
                    .HasColumnName("Tour_Name")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Act)
                    .WithMany(p => p.Tournaments)
                    .HasForeignKey(d => d.ActId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tour_Act_ID");

                entity.HasOne(d => d.MemberInfoNavigation)
                    .WithMany(p => p.Tournaments)
                    .HasForeignKey(d => d.MemberInfo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tour_Member_Info");
            });

            modelBuilder.Entity<Trainer>(entity =>
            {
                entity.HasKey(e => e.TrainerCode)
                    .HasName("PK_Trainer_Code");

                entity.Property(e => e.TrainerCode).HasColumnName("Trainer_Code");

                entity.Property(e => e.TrainerAct).HasColumnName("Trainer_Act");

                entity.Property(e => e.TrainerActive)
                    .HasColumnName("Trainer_Active")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.TrainerInfo).HasColumnName("Trainer_Info");

                entity.Property(e => e.TrainerStartDay)
                    .HasColumnName("Trainer_StartDay")
                    .HasColumnType("date");

                entity.HasOne(d => d.TrainerActNavigation)
                    .WithMany(p => p.Trainer)
                    .HasForeignKey(d => d.TrainerAct)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Trainer_Act");

                entity.HasOne(d => d.TrainerInfoNavigation)
                    .WithMany(p => p.Trainer)
                    .HasForeignKey(d => d.TrainerInfo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Trainer_Info");
            });

            modelBuilder.Entity<TrainerSalary>(entity =>
            {
                entity.Property(e => e.TrainerSalaryId).HasColumnName("TrainerSalary_ID");

                entity.Property(e => e.MemberCount)
                    .HasColumnName("Member_Count")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Salary).HasDefaultValueSql("((0))");

                entity.Property(e => e.SalaryDate)
                    .HasColumnName("Salary_Date")
                    .HasColumnType("date");

                entity.Property(e => e.SchoolId).HasColumnName("School_ID");

                entity.Property(e => e.TrainerCode).HasColumnName("Trainer_Code");

                entity.HasOne(d => d.School)
                    .WithMany(p => p.TrainerSalary)
                    .HasForeignKey(d => d.SchoolId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TrainerSalary_School_ID");

                entity.HasOne(d => d.TrainerCodeNavigation)
                    .WithMany(p => p.TrainerSalary)
                    .HasForeignKey(d => d.TrainerCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TrainerSalary_Trainer_Code");
            });
        }
    }
}
