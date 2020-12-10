﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace jctravel01.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TravelContainer : DbContext
    {
        public TravelContainer()
            : base("name=TravelContainer")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<HRInfo> HRInfo { get; set; }
        public virtual DbSet<Airline> Airline { get; set; }
        public virtual DbSet<AirlineOffice> AirlineOffice { get; set; }
        public virtual DbSet<AirportInfo> AirportInfo { get; set; }
        public virtual DbSet<AmosphIndex> AmosphIndex { get; set; }
        public virtual DbSet<Authorze_index> Authorze_index { get; set; }
        public virtual DbSet<City03> City03 { get; set; }
        public virtual DbSet<CityDistrict> CityDistrict { get; set; }
        public virtual DbSet<CoIndex> CoIndex { get; set; }
        public virtual DbSet<Country_City_index> Country_City_index { get; set; }
        public virtual DbSet<Country01> Country01 { get; set; }
        public virtual DbSet<DepIndex> DepIndex { get; set; }
        public virtual DbSet<DiningStyIndex> DiningStyIndex { get; set; }
        public virtual DbSet<Division> Division { get; set; }
        public virtual DbSet<Hotel> Hotel { get; set; }
        public virtual DbSet<Hotel_Theme> Hotel_Theme { get; set; }
        public virtual DbSet<Hotel_Type> Hotel_Type { get; set; }
        public virtual DbSet<HotelFaci_index> HotelFaci_index { get; set; }
        public virtual DbSet<HotelFacility> HotelFacility { get; set; }
        public virtual DbSet<HotelOutLook> HotelOutLook { get; set; }
        public virtual DbSet<HotelSer_index> HotelSer_index { get; set; }
        public virtual DbSet<HotelService> HotelService { get; set; }
        public virtual DbSet<Img> Img { get; set; }
        public virtual DbSet<JobGruopIndex> JobGruopIndex { get; set; }
        public virtual DbSet<JobLevelIndex> JobLevelIndex { get; set; }
        public virtual DbSet<logInLog> logInLog { get; set; }
        public virtual DbSet<NearbyFacility> NearbyFacility { get; set; }
        public virtual DbSet<NearbyFcai_index> NearbyFcai_index { get; set; }
        public virtual DbSet<OutLook_index> OutLook_index { get; set; }
        public virtual DbSet<PasMng> PasMng { get; set; }
        public virtual DbSet<PermiGruop> PermiGruop { get; set; }
        public virtual DbSet<PermiIndex> PermiIndex { get; set; }
        public virtual DbSet<PermiMng> PermiMng { get; set; }
        public virtual DbSet<Res_Theme> Res_Theme { get; set; }
        public virtual DbSet<Res_Type> Res_Type { get; set; }
        public virtual DbSet<ResAmos> ResAmos { get; set; }
        public virtual DbSet<ResDining> ResDining { get; set; }
        public virtual DbSet<Restaurant> Restaurant { get; set; }
        public virtual DbSet<RoomFaci_index> RoomFaci_index { get; set; }
        public virtual DbSet<RoomFacility> RoomFacility { get; set; }
        public virtual DbSet<RoomType> RoomType { get; set; }
        public virtual DbSet<RoomType_index> RoomType_index { get; set; }
        public virtual DbSet<Scenery> Scenery { get; set; }
        public virtual DbSet<Scenery_Theme> Scenery_Theme { get; set; }
        public virtual DbSet<Scenery_Type> Scenery_Type { get; set; }
        public virtual DbSet<State02> State02 { get; set; }
        public virtual DbSet<Theme_index> Theme_index { get; set; }
        public virtual DbSet<TourBure> TourBure { get; set; }
        public virtual DbSet<Type_index> Type_index { get; set; }
        public virtual DbSet<UpDivision> UpDivision { get; set; }
    }
}