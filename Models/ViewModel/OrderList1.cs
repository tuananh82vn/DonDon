using System;
using Android.OS;
using Java.Interop;

namespace DonDon
{
	public sealed class OrderList1 : Java.Lang.Object, IParcelable 
	{
		public OrderList1(){
		}

		public OrderList1(int id, string stockname , double shouldnumber , double stocknumber, double ordernumber, string unit, int isskip){
			Id = id;
			StockName = stockname;
			ShouldNumber = shouldnumber;
			StockNumber = stocknumber;
			OrderNumber = ordernumber;
			Unit = unit;
			IsSkip = isskip;
		}

		private OrderList1(Parcel parcel)
		{
			Id = parcel.ReadInt();
			StockName = parcel.ReadString();
			ShouldNumber = parcel.ReadDouble();
			StockNumber = parcel.ReadDouble();
			OrderNumber = parcel.ReadDouble();
			Unit = parcel.ReadString();
			IsSkip = parcel.ReadInt();

		}


		public int 		Id { get ; set;	}
		public string 	StockName {get ; set; }
		public double 		ShouldNumber {get ; set;}
		public double 		StockNumber {get ; set;}
		public double 		OrderNumber {get ; set;}
		public string 	Unit { get ; set;}
		public int IsSkip { get ; set;}


		public int DescribeContents()
		{
			return 0;
		}

		// Save this instance's values to the parcel
		public void WriteToParcel(Parcel dest, ParcelableWriteFlags flags)
		{
			dest.WriteInt(Id);
			dest.WriteString(StockName);
			dest.WriteDouble(ShouldNumber);
			dest.WriteDouble(StockNumber);
			dest.WriteDouble(OrderNumber);
			dest.WriteString(Unit);
			dest.WriteInt(IsSkip);
		}

		// The creator creates an instance of the specified object
		private static readonly GenericParcelableCreator<OrderList1> _creator
		= new GenericParcelableCreator<OrderList1>((parcel) => new OrderList1(parcel));

		[ExportField("CREATOR")]
		public static GenericParcelableCreator<OrderList1> GetCreator()
		{
			return _creator;
		}

	}

	public sealed class GenericParcelableCreator<T> : Java.Lang.Object, IParcelableCreator
		where T : Java.Lang.Object, new()
	{
		private readonly Func<Parcel, T> _createFunc;

		/// <summary>
		/// Initializes a new instance of the <see cref="ParcelableDemo.GenericParcelableCreator`1"/> class.
		/// </summary>
		/// <param name='createFromParcelFunc'>
		/// Func that creates an instance of T, populated with the values from the parcel parameter
		/// </param>
		public GenericParcelableCreator(Func<Parcel, T> createFromParcelFunc)
		{
			_createFunc = createFromParcelFunc;
		}

		#region IParcelableCreator Implementation

		public Java.Lang.Object CreateFromParcel(Parcel source)
		{
			return _createFunc(source);
		}

		public Java.Lang.Object[] NewArray(int size)
		{
			return new T[size];
		}

		#endregion
	}

}

