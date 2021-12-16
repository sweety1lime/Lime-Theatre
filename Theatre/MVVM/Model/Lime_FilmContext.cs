using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Theatre.MVVM.Model
{
    public partial class Lime_FilmContext : DbContext
    {
        public Lime_FilmContext()
        {
        }

        public Lime_FilmContext(DbContextOptions<Lime_FilmContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<AgeRating>(entity =>
            {
                entity.HasKey(e => e.IdRating)
                    .HasName("PK_Rating");

                entity.ToTable("Age_Rating");

                entity.Property(e => e.IdRating).HasColumnName("ID_Rating");

                entity.Property(e => e.NameRating)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("Name_Rating");
            });

            modelBuilder.Entity<BookKeeping>(entity =>
            {
                entity.HasKey(e => e.IdNote);

                entity.ToTable("BookKeeping");

                entity.Property(e => e.IdNote).HasColumnName("ID_Note");

                entity.Property(e => e.PaymentId).HasColumnName("Payment_ID");

                entity.Property(e => e.RecoveryId).HasColumnName("Recovery_ID");

          
            });

            modelBuilder.Entity<Caffe>(entity =>
            {
                entity.HasKey(e => e.IdCaffe);

                entity.ToTable("Caffe");

                entity.Property(e => e.IdCaffe).HasColumnName("ID_Caffe");

                entity.Property(e => e.EmployeeId).HasColumnName("Employee_ID");

                entity.Property(e => e.Goods)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

             
            });

            modelBuilder.Entity<CaffeCheck>(entity =>
            {
                entity.HasKey(e => e.IdCheck)
                    .HasName("PK_CheckCaffe");

                entity.ToTable("Caffe_Check");

                entity.Property(e => e.IdCheck).HasColumnName("ID_Check");

                entity.Property(e => e.CaffeId).HasColumnName("Caffe_ID");

                entity.Property(e => e.CountGoods).HasColumnName("Count_Goods");

                entity.Property(e => e.DatePayment)
                    .HasColumnType("datetime")
                    .HasColumnName("Date_Payment");

                entity.Property(e => e.TypePaymentId).HasColumnName("TypePayment_ID");

          

              
            });

            modelBuilder.Entity<Cashbox>(entity =>
            {
                entity.HasKey(e => e.IdCashBox);

                entity.ToTable("Cashbox");

                entity.Property(e => e.IdCashBox).HasColumnName("ID_CashBox");

                entity.Property(e => e.EmployeeId).HasColumnName("Employee_ID");

                entity.Property(e => e.TicketId).HasColumnName("Ticket_ID");

          
            });

            modelBuilder.Entity<Check>(entity =>
            {
                entity.HasKey(e => e.IdCheck);

                entity.ToTable("Check");

                entity.Property(e => e.IdCheck).HasColumnName("ID_Check");

                entity.Property(e => e.CashBoxId).HasColumnName("CashBox_ID");

                entity.Property(e => e.CountGoods).HasColumnName("Count_Goods");

                entity.Property(e => e.DatePayment)
                    .HasColumnType("datetime")
                    .HasColumnName("Date_Payment");

                entity.Property(e => e.TypePaymentId).HasColumnName("TypePayment_ID");

        
            });

            modelBuilder.Entity<CinemaHall>(entity =>
            {
                entity.HasKey(e => e.IdHall);

                entity.ToTable("Cinema_Hall");

                entity.Property(e => e.IdHall).HasColumnName("ID_Hall");

                entity.Property(e => e.CountSeat).HasColumnName("Count_Seat");

                entity.Property(e => e.LeftSeat).HasColumnName("Left_Seat");

                entity.Property(e => e.NameHall)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("Name_Hall");

                entity.Property(e => e.TypeId).HasColumnName("Type_ID");

  
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.IdEmployee);

                entity.ToTable("Employee");

                entity.Property(e => e.IdEmployee).HasColumnName("ID_Employee");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.MiddleName)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.PostId).HasColumnName("Post_ID");

                entity.Property(e => e.UserId).HasColumnName("User_ID");

      
            });

            modelBuilder.Entity<Film>(entity =>
            {
                entity.HasKey(e => e.IdFilm)
                    .HasName("PK_Film");

                entity.Property(e => e.IdFilm).HasColumnName("ID_Film");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.DurationFilm)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("Duration_Film");

                entity.Property(e => e.GenreId).HasColumnName("Genre_ID");

                entity.Property(e => e.NameFlim)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("Name_Flim");

                entity.Property(e => e.RatingId).HasColumnName("Rating_ID");

                entity.Property(e => e.StudioId).HasColumnName("Studio_ID");

       
            });

            modelBuilder.Entity<FilmGenre>(entity =>
            {
                entity.HasKey(e => e.IdGenre)
                    .HasName("PK_Genre");

                entity.ToTable("Film_genre");

                entity.Property(e => e.IdGenre).HasColumnName("ID_Genre");

                entity.Property(e => e.NameGenre)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("Name_Genre");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.IdPayment);

                entity.ToTable("Payment");

                entity.Property(e => e.IdPayment).HasColumnName("ID_Payment");

                entity.Property(e => e.DatePayment)
                    .HasColumnType("datetime")
                    .HasColumnName("Date_Payment");

                entity.Property(e => e.EmployeeId).HasColumnName("Employee_ID");

                entity.Property(e => e.SumPayment).HasColumnName("Sum_Payment");

     
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasKey(e => e.IdPost);

                entity.ToTable("Post");

                entity.Property(e => e.IdPost).HasColumnName("ID_Post");

                entity.Property(e => e.NamePost)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("Name_Post");
            });

            modelBuilder.Entity<Rate>(entity =>
            {
                entity.HasKey(e => e.IdRate);

                entity.ToTable("Rate");

                entity.Property(e => e.IdRate).HasColumnName("ID_Rate");

                entity.Property(e => e.NameRate)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("Name_Rate");
            });

            modelBuilder.Entity<Recovery>(entity =>
            {
                entity.HasKey(e => e.IdRecovery);

                entity.ToTable("Recovery");

                entity.Property(e => e.IdRecovery).HasColumnName("ID_Recovery");

                entity.Property(e => e.DateRecovery)
                    .HasColumnType("datetime")
                    .HasColumnName("Date_Recovery");

                entity.Property(e => e.EmployeeId).HasColumnName("Employee_ID");

                entity.Property(e => e.NameRecovery)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("Name_Recovery");

                entity.Property(e => e.SumRecovery).HasColumnName("Sum_Recovery");

           
            });

            modelBuilder.Entity<RentCinema>(entity =>
            {
                entity.HasKey(e => e.IdRent);

                entity.ToTable("Rent_Cinema");

                entity.Property(e => e.IdRent).HasColumnName("ID_Rent");

                entity.Property(e => e.EmployeeId).HasColumnName("Employee_ID");

                entity.Property(e => e.FilmId).HasColumnName("Film_ID");

                entity.Property(e => e.RentDuration)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("Rent_Duration");

     
            });

            modelBuilder.Entity<Row>(entity =>
            {
                entity.HasKey(e => e.IdRow);

                entity.ToTable("Row");

                entity.Property(e => e.IdRow).HasColumnName("ID_Row");

                entity.Property(e => e.CategoryRow)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("Category_Row");
            });

            modelBuilder.Entity<Seat>(entity =>
            {
                entity.HasKey(e => e.IdSeat);

                entity.ToTable("Seat");

                entity.Property(e => e.IdSeat).HasColumnName("ID_Seat");

                entity.Property(e => e.CategorySeat)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("Category_Seat");
            });

            modelBuilder.Entity<Session>(entity =>
            {
                entity.HasKey(e => e.IdSession);

                entity.ToTable("Session");

                entity.Property(e => e.IdSession).HasColumnName("ID_Session");

                entity.Property(e => e.DateTime).HasColumnType("datetime");

                entity.Property(e => e.FilmId).HasColumnName("Film_ID");

                entity.Property(e => e.HallId).HasColumnName("Hall_ID");

         
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.HasKey(e => e.IdStatus);

                entity.ToTable("Status");

                entity.Property(e => e.IdStatus).HasColumnName("ID_Status");

                entity.Property(e => e.StatusName)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("Status_Name");
            });

            modelBuilder.Entity<Studio>(entity =>
            {
                entity.HasKey(e => e.IdStudio);

                entity.ToTable("Studio");

                entity.Property(e => e.IdStudio).HasColumnName("ID_Studio");

                entity.Property(e => e.NameStudio)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("Name_Studio");
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasKey(e => e.IdTicket)
                    .HasName("PK_Ticket");

                entity.Property(e => e.IdTicket).HasColumnName("ID_Ticket");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.HallId).HasColumnName("Hall_ID");

                entity.Property(e => e.RateId).HasColumnName("Rate_ID");

                entity.Property(e => e.RowId).HasColumnName("Row_ID");

                entity.Property(e => e.SeatId).HasColumnName("Seat_ID");

                entity.Property(e => e.StatusId).HasColumnName("Status_ID");

      
            });

            modelBuilder.Entity<TypeHall>(entity =>
            {
                entity.HasKey(e => e.IdType)
                    .HasName("PK_TypeHall");

                entity.ToTable("Type_Hall");

                entity.Property(e => e.IdType).HasColumnName("ID_Type");

                entity.Property(e => e.NameType)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("Name_Type");
            });

            modelBuilder.Entity<TypePayment>(entity =>
            {
                entity.HasKey(e => e.IdType)
                    .HasName("PK_TypePayment");

                entity.ToTable("Type_Payment");

                entity.Property(e => e.IdType).HasColumnName("ID_Type");

                entity.Property(e => e.NameType)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("Name_Type");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.IdUser);

                entity.ToTable("User");

                entity.Property(e => e.IdUser).HasColumnName("ID_User");

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.PostId).HasColumnName("Post_ID");

    
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
