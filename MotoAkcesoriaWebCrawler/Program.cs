using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace MotoAkcesoriaWebCrawler
{
	static class Program
	{
		private const string ServerName = @"http://www.moto-akcesoria.pl/product_info.php?products_id=";
		private static int _stopId;
		private static int _startId;

		public static bool ThreadPause;
		public static bool ThreadStop;

		private static MySqlConnection _mySqlConnect;

		private static MotoAkcesoriaWebCrawler _form;

		public static bool OpenMySqlConnection(string server, string database, string user, string password)
		{
			var mySqlConnection =
				new MySqlConnection(String.Format("Server={0};Database={1};Uid={2};Pwd={3}", server, database, user, password));
			try
			{
				mySqlConnection.Open();
			}
			catch(MySqlException)
			{
				return false;
			}
			_mySqlConnect = mySqlConnection;

			return true;
		}

		public static void CloseMySqlConnection()
		{
			_mySqlConnect.Close();
		}

		public static void SetStartAndStopId(string startid, string stopid)
		{
			_stopId = Convert.ToInt32(stopid);
			_startId = Convert.ToInt32(startid);
		}

		public static void Start()
		{
			RepairsInDatabase();

			for (var i = _startId; i < _stopId; i++)
			{
				_mySqlConnect.Ping();

				if (Application.OpenForms.Count != 0)
					_form = (MotoAkcesoriaWebCrawler) Application.OpenForms[0];
				else
					Thread.CurrentThread.Abort();

				if (ThreadStop)
				{
					break;
				}
				if (ThreadPause)
				{
					--i;
					continue;
				}

				_form.UpdateListBoxSelectedValue();
				_form.UpdateProgressBar(i);

				var request = (HttpWebRequest)WebRequest.Create(ServerName + i);
				var response = (HttpWebResponse)request.GetResponse();
				var input = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("iso-8859-2"));
				string temp;
				while ((temp = input.ReadLine()) != null)
				{
					if (!temp.Contains("<body")) continue;

					temp = input.ReadToEnd();

					var noProductReg = new Regex("Nie znaleziono produktu!");
					var resultOfRegex = noProductReg.Match(temp);

					if (resultOfRegex.Success)
						_form.UpdateListBoxAdd(String.Format("{0}. Nie ma produktu", i));

					if (!resultOfRegex.Success)
					{
						//---------------------------------- "Wyciąganie" kategorii produktu
						var categoriesForProduct = GetCategories(temp);
						//---------------------------------- Koniec "wyciągania" kategorii produktu

						//---------------------------------- Sprawdzanie czy kategoria znajduje się w bazie, dodanie kategorii do bazy
						var productsCategory = CheckAndAddCategories(categoriesForProduct);
						//---------------------------------- Koniec sprawdzania czy kategoria znajduje się w bazie, dodawania kategorii do bazy

						//---------------------------------- "Wyciąganie" nazwy producenta
						var manufacturer = GetManufacturer(temp);
						//---------------------------------- Koniec "wyciągania" nazwy producenta

						//---------------------------------- "Wyciąganie" głównego opisu
						var productMainDescr = GetDescription(temp);
						//---------------------------------- Koniec "wyciągania" głównego opisu

						//---------------------------------- "Wyciąganie" numerów
						var productSubNumbers = GetProductNumbers(temp);
						//---------------------------------- Koniec "wyciągania" numerów

						//---------------------------------- "Wyciąganie" podproduktów
						var productSubDescriptions = GetSubProductsDescr(temp);
						ShowInFormationInListBox(i, productSubDescriptions);
						//---------------------------------- Koniec "wyciągania" podproduktów

						//---------------------------------- Sprawdzanie czy produkt znajduje się w bazie, dodanie produktu
						var productIds = CheckAndAddProducts(productSubNumbers, productMainDescr, productSubDescriptions);
						//---------------------------------- Koniec sprawdzania czy produkt znajduje się w bazie, dodawania produktu

						//---------------------------------- Podpięcie produktów do innych tabel
						WiringProductsToCategoriesAndManufacturer(productIds, productsCategory, manufacturer);
						//---------------------------------- Koniec podpinana produktów do innych tabel
					}
				}
				request.Abort();
				Thread.Sleep(RandomNumber(100, 250));
			}
		}

		private static void RepairsInDatabase()
		{
			//za mała wartość ustawiona w nazwie - 64 domyślnie. Zmieniamy to na 256
			const string alterQuery = "ALTER TABLE jos_vm_product CHANGE product_name product_name VARCHAR(256)";
			var mySqlCommand = new MySqlCommand(alterQuery, _mySqlConnect);
			mySqlCommand.ExecuteNonQuery();
		}

		private static int RandomNumber(int min, int max)
		{
			var random = new Random();
			return random.Next(min, max);
		}

		private static void ShowInFormationInListBox(int i, IList<string> productSubDescriptions)
		{
			for (var j = 0; j < productSubDescriptions.Count; j++)
			{
				switch (j)
				{
					case 0:
						_form.UpdateListBoxAdd(String.Format("{0}a. Znaleziono '{1}'", i, productSubDescriptions[j]));
						break;
					case 1:
						_form.UpdateListBoxAdd(String.Format("{0}b. Znaleziono '{1}'", i, productSubDescriptions[j]));
						break;
					case 2:
						_form.UpdateListBoxAdd(String.Format("{0}c. Znaleziono '{1}'", i, productSubDescriptions[j]));
						break;
					case 3:
						_form.UpdateListBoxAdd(String.Format("{0}d. Znaleziono '{1}'", i, productSubDescriptions[j]));
						break;
					case 4:
						_form.UpdateListBoxAdd(String.Format("{0}e. Znaleziono '{1}'", i, productSubDescriptions[j]));
						break;
					case 5:
						_form.UpdateListBoxAdd(String.Format("{0}f. Znaleziono '{1}'", i, productSubDescriptions[j]));
						break;
					case 6:
						_form.UpdateListBoxAdd(String.Format("{0}g. Znaleziono '{1}'", i, productSubDescriptions[j]));
						break;
					case 7:
						_form.UpdateListBoxAdd(String.Format("{0}h. Znaleziono '{1}'", i, productSubDescriptions[j]));
						break;
					case 8:
						_form.UpdateListBoxAdd(String.Format("{0}i. Znaleziono '{1}'", i, productSubDescriptions[j]));
						break;
					case 9:
						_form.UpdateListBoxAdd(String.Format("{0}j. Znaleziono '{1}'", i, productSubDescriptions[j]));
						break;
					case 10:
						_form.UpdateListBoxAdd(String.Format("{0}k. Znaleziono '{1}'", i, productSubDescriptions[j]));
						break;
					case 11:
						_form.UpdateListBoxAdd(String.Format("{0}l. Znaleziono '{1}'", i, productSubDescriptions[j]));
						break;
					case 12:
						_form.UpdateListBoxAdd(String.Format("{0}m. Znaleziono '{1}'", i, productSubDescriptions[j]));
						break;
					case 13:
						_form.UpdateListBoxAdd(String.Format("{0}n. Znaleziono '{1}'", i, productSubDescriptions[j]));
						break;
					case 14:
						_form.UpdateListBoxAdd(String.Format("{0}o. Znaleziono '{1}'", i, productSubDescriptions[j]));
						break;
					case 15:
						_form.UpdateListBoxAdd(String.Format("{0}p. Znaleziono '{1}'", i, productSubDescriptions[j]));
						break;
					case 16:
						_form.UpdateListBoxAdd(String.Format("{0}q. Znaleziono '{1}'", i, productSubDescriptions[j]));
						break;
					case 17:
						_form.UpdateListBoxAdd(String.Format("{0}r. Znaleziono '{1}'", i, productSubDescriptions[j]));
						break;
					case 18:
						_form.UpdateListBoxAdd(String.Format("{0}s. Znaleziono '{1}'", i, productSubDescriptions[j]));
						break;
					case 19:
						_form.UpdateListBoxAdd(String.Format("{0}t. Znaleziono '{1}'", i, productSubDescriptions[j]));
						break;
					case 20:
						_form.UpdateListBoxAdd(String.Format("{0}u. Znaleziono '{1}'", i, productSubDescriptions[j]));
						break;
					case 21:
						_form.UpdateListBoxAdd(String.Format("{0}v. Znaleziono '{1}'", i, productSubDescriptions[j]));
						break;
					case 22:
						_form.UpdateListBoxAdd(String.Format("{0}w. Znaleziono '{1}'", i, productSubDescriptions[j]));
						break;
					case 23:
						_form.UpdateListBoxAdd(String.Format("{0}x. Znaleziono '{1}'", i, productSubDescriptions[j]));
						break;
					case 24:
						_form.UpdateListBoxAdd(String.Format("{0}y. Znaleziono '{1}'", i, productSubDescriptions[j]));
						break;
					case 25:
						_form.UpdateListBoxAdd(String.Format("{0}z. Znaleziono '{1}'", i, productSubDescriptions[j]));
						break;
				}
			}
		}

		private static void WiringProductsToCategoriesAndManufacturer(IEnumerable<int> productIds, int productsCategory, string manufacturer)
		{
			foreach (var productId in productIds)
			{
				var selectQuery = "select category_id, product_id, product_list from jos_vm_product_category_xref where product_id = '" + productId + "'";
				var mySqlCommand = new MySqlCommand(selectQuery, _mySqlConnect);
				var mySqlReader = mySqlCommand.ExecuteReader();

				if (!mySqlReader.Read())
				{
					mySqlReader.Close();
					var insertQuery = "insert into jos_vm_product_category_xref (category_id, product_id, product_list) values (" + productsCategory + ", " + productId + ", 1)";
					new MySqlCommand(insertQuery, _mySqlConnect).ExecuteNonQuery();
				}

				if (!mySqlReader.IsClosed) mySqlReader.Close();

				if (!String.IsNullOrEmpty(manufacturer))
				{
					selectQuery = "select manufacturer_id, mf_name, mf_email, mf_desc, mf_category_id, mf_url from jos_vm_manufacturer where mf_name = '" + manufacturer + "'";
					mySqlCommand = new MySqlCommand(selectQuery, _mySqlConnect);
					mySqlReader = mySqlCommand.ExecuteReader();

					int manufacturerId;
					if (!mySqlReader.Read())
					{
						mySqlReader.Close();
						var insertQuery =
							"insert into jos_vm_manufacturer (mf_name, mf_email, mf_desc, mf_category_id, mf_url) values ('" + manufacturer + "', '', '', 1, '')" ;
						var insertCommand = new MySqlCommand(insertQuery, _mySqlConnect);
						insertCommand.ExecuteNonQuery();
						manufacturerId = (int) insertCommand.LastInsertedId;
					}
					else
					{
						manufacturerId = mySqlReader.GetInt32("manufacturer_id");
					}

					if (!mySqlReader.IsClosed) mySqlReader.Close();

					selectQuery = "select product_id, manufacturer_id from jos_vm_product_mf_xref where product_id = '" + productId + "'";
					mySqlCommand = new MySqlCommand(selectQuery, _mySqlConnect);
					mySqlReader = mySqlCommand.ExecuteReader();

					if (!mySqlReader.Read())
					{
						mySqlReader.Close();
						var insertQuery = "insert into jos_vm_product_mf_xref (product_id, manufacturer_id) values (" + productId + ", " + manufacturerId +")";
						new MySqlCommand(insertQuery, _mySqlConnect).ExecuteNonQuery();
					}

					if (!mySqlReader.IsClosed) mySqlReader.Close();
				}
				else
				{
					selectQuery = "select product_id, manufacturer_id from jos_vm_product_mf_xref where product_id = '" + productId + "'";
					mySqlCommand = new MySqlCommand(selectQuery, _mySqlConnect);
					mySqlReader = mySqlCommand.ExecuteReader();

					if (!mySqlReader.Read())
					{
						mySqlReader.Close();
						var insertQuery = "insert into jos_vm_product_mf_xref (product_id, manufacturer_id) values (" + productId + ", 1)";
						new MySqlCommand(insertQuery, _mySqlConnect).ExecuteNonQuery();
					}

					if (!mySqlReader.IsClosed) mySqlReader.Close();
				}
			}
		}

		private static IEnumerable<int> CheckAndAddProducts(IList<string> productSubNumbers, string productMainDescr, IList<string> productSubDescriptions)
		{
			var productIds = new List<int>();
			var productAttributes = new List<string>();

			if (productSubNumbers.Count > 1)
			{
				var attributes = "Rozmiar";

				var productNumberLength = productSubNumbers[0].Length - 2;
				string productNumber = productSubNumbers[0].Substring(0, productNumberLength);

				const string attributesRegex = "((rozm\\.)|(Rozmiar)|(rozmiar))\\s*\\w*";
				foreach (var productSubDescription in productSubDescriptions)
				{
					var temp = new Regex(attributesRegex).Match(productSubDescription).Value;
					try 
					{
						productAttributes.Add(temp.Split(' ')[1]);
					}
					catch(Exception)
					{
						try
						{
							productAttributes.Add(temp.Split('.')[1]);
						}
						catch(Exception)
						{
							var tempSubDescr = productSubDescription.Split(' ');
							productAttributes.Add(tempSubDescr[tempSubDescr.Length - 1]);
						}
					}
				}

				var productTemp = new Regex(attributesRegex).Match(productSubDescriptions[0]).Value;
				string productName = "";
				try 
				{
					productName = productSubDescriptions[0].Replace(productTemp, "");
				}
				catch(Exception)
				{
					var tempProductNameAll = productSubDescriptions[0].Split(' ');
					var tempProductName = "";
					for (var i = 0; i < tempProductNameAll.Length - 1; i++)
					{
						tempProductName += tempProductNameAll[i] + " ";
					}

					productName = tempProductName;
				}

				for (var i = 0; i < productSubNumbers.Count; i++)
				{
					attributes += String.Format(",({0}) {1}", productSubNumbers[i], productAttributes[i]);
				}

				var selectQuery = "select * from jos_vm_product where product_sku = " + productNumber;
				selectQuery = productSubNumbers.Aggregate(selectQuery, (current, productSubNumber) => current + (" or attribute like \"%" + productSubNumber + "%\""));

				var selectCommand = new MySqlCommand(selectQuery, _mySqlConnect).ExecuteReader();

				if (!selectCommand.Read())
				{
					selectCommand.Close();

					var insertQuery = String.Format("insert into jos_vm_product (vendor_id, product_parent_id, product_sku, product_s_desc, product_desc, product_thumb_image, product_full_image, product_publish, product_weight, product_weight_uom, product_length, product_width, product_height, product_lwh_uom, product_url, product_in_stock, product_available_date, product_availability, product_special, product_discount_id, ship_code_id, cdate, mdate, product_name, product_sales, attribute, custom_attribute, product_tax_id, product_unit, product_packaging, child_options, quantity_options, child_option_ids, product_order_levels) VALUES (1, 0, {0}, '', '{1}', '', '', 'Y', '0.0000', 'kg', '0.0000', '0.0000', '0.0000', 'cm', '', 0, 1279843200, '', 'N', 0, NULL, 1279922325, 1279922577, '{2}', 0, '{3}', '', 3, 'szt.', 0, 'N,N,N,N,N,Y,20%,10%,', 'none,0,0,1', '', '0,0')", productNumber, productMainDescr, productName, attributes);
					var insertCommand = new MySqlCommand(insertQuery, _mySqlConnect);
					insertCommand.ExecuteNonQuery();
					productIds.Add((int)insertCommand.LastInsertedId);
				}
				else
				{
					productIds.Add(selectCommand.GetInt32("product_id"));
					selectCommand.Close();
				}
			}
			else
			{
				for (var j = 0; j < productSubNumbers.Count; j++)
				{
					var selectQuery = "select * from jos_vm_product where product_sku = " + productSubNumbers[j];
					selectQuery = productSubNumbers.Aggregate(selectQuery, (current, productSubNumber) => current + (" or attribute like \"%" + productSubNumber + "%\""));

					var selectCommand = new MySqlCommand(selectQuery, _mySqlConnect).ExecuteReader();

					if (!selectCommand.Read())
					{
						selectCommand.Close();

						var insertQuery = String.Format("insert into jos_vm_product (vendor_id, product_parent_id, product_sku, product_s_desc, product_desc, product_thumb_image, product_full_image, product_publish, product_weight, product_weight_uom, product_length, product_width, product_height, product_lwh_uom, product_url, product_in_stock, product_available_date, product_availability, product_special, product_discount_id, ship_code_id, cdate, mdate, product_name, product_sales, attribute, custom_attribute, product_tax_id, product_unit, product_packaging, child_options, quantity_options, child_option_ids, product_order_levels) VALUES (1, 0, {0}, '', '{1}', '', '', 'Y', '0.0000', 'kg', '0.0000', '0.0000', '0.0000', 'cm', '', 0, 1279843200, '', 'N', 0, NULL, 1279922325, 1279922577, '{2}', 0, '', '', 3, 'szt.', 0, 'N,N,N,N,N,Y,20%,10%,', 'none,0,0,1', '', '0,0')", productSubNumbers[j], productMainDescr, productSubDescriptions[j]);
						var insertCommand = new MySqlCommand(insertQuery, _mySqlConnect);
						insertCommand.ExecuteNonQuery();
						productIds.Add((int)insertCommand.LastInsertedId);
					}
					else
					{
						productIds.Add(selectCommand.GetInt32("product_id"));
						selectCommand.Close();
						continue;
					}
				}
			}

			return productIds;
		}

		private static int CheckAndAddCategories(IList<string> categoriesForProduct)
		{
			var productsCategory = 0;
			var parentCategoryId = 0;

			for (var j = 0; j < categoriesForProduct.Count; j++)
			{
				var selectQuery = "select category_id, category_name from jos_vm_category where category_name = '" + categoriesForProduct[j] + "'";

				var mySqlCommand = new MySqlCommand(selectQuery, _mySqlConnect);
				var mySqlReader = mySqlCommand.ExecuteReader();

				switch (j)
				{
					case 0:
						parentCategoryId = AddCategory(j, categoriesForProduct, selectQuery, mySqlReader);
						break;

					case 1:
						productsCategory = AddSubCategory(j, categoriesForProduct, selectQuery, mySqlReader, parentCategoryId);
						break;
				}

				mySqlReader.Close();
			}

			return productsCategory;
		}

		private static int AddSubCategory(int j, IList<string> categoriesForProduct, string selectQuery, MySqlDataReader mySqlReader, int parentCategoryId)
		{
			int productsCategory;
			if (!mySqlReader.Read())
			{
				mySqlReader.Close();

				var insertCommand = "insert into jos_vm_category (vendor_id, category_name, category_description, category_thumb_image, category_full_image, category_publish, cdate, mdate, category_browsepage, products_per_row, category_flypage, list_order) VALUES (1, '" + categoriesForProduct[j] + "', '', '', '', 'Y', 1279922222, 1279922222, 'managed', 1, 'flypage.tpl', 1)";
				var mySqlCommandInsert = new MySqlCommand(insertCommand, _mySqlConnect);
				mySqlCommandInsert.ExecuteNonQuery();

				var childCategoryId = mySqlCommandInsert.LastInsertedId;

				insertCommand =
					string.Format("insert into jos_vm_category_xref (category_parent_id, category_child_id, category_list) VALUES ({0}, {1}, NULL)", parentCategoryId, childCategoryId);
				var mySqlCommandInsert2 = new MySqlCommand(insertCommand, _mySqlConnect);
				mySqlCommandInsert2.ExecuteNonQuery();

				productsCategory = (int)childCategoryId;
			}
			else
			{
				mySqlReader.Close();

				var selectCommand = new MySqlCommand(selectQuery, _mySqlConnect);
				var selectTemp = selectCommand.ExecuteReader();
				selectTemp.Read();
				productsCategory = selectTemp.GetInt32("category_id");
				selectTemp.Close();
			}

			return productsCategory;
		}

		private static int AddCategory(int j, IList<string> categoriesForProduct, string selectQuery, MySqlDataReader mySqlReader)
		{
			int parentCategoryId;
			//if (mySqlReader.Read() || categoriesForProduct[j] != mySqlReader.GetString("category_name"))
			if (!mySqlReader.Read())
			{
				mySqlReader.Close();

				var insertCommand = "insert into jos_vm_category (vendor_id, category_name, category_description, category_thumb_image, category_full_image, category_publish, cdate, mdate, category_browsepage, products_per_row, category_flypage, list_order) VALUES (1, '" + categoriesForProduct[j] + "', '', '', '', 'Y', 1279922222, 1279922222, 'managed', 1, 'flypage.tpl', 1)";
				var mySqlCommandInsert = new MySqlCommand(insertCommand, _mySqlConnect);
				mySqlCommandInsert.ExecuteNonQuery();

				//var selectCommand = new MySqlCommand(selectQuery, _mySqlConnect);
				//parentCategoryId = selectCommand.ExecuteReader().GetInt32("category_id");

				parentCategoryId = (int)mySqlCommandInsert.LastInsertedId;

				insertCommand =
					"insert into jos_vm_category_xref (category_parent_id, category_child_id, category_list) VALUES (0, " + parentCategoryId + ", NULL)";
				var mySqlCommandInsert2 = new MySqlCommand(insertCommand, _mySqlConnect);
				mySqlCommandInsert2.ExecuteNonQuery();
			}
			else
			{
				mySqlReader.Close();

				var selectCommand = new MySqlCommand(selectQuery, _mySqlConnect);
				var selectTemp = selectCommand.ExecuteReader();
				selectTemp.Read();
				parentCategoryId = selectTemp.GetInt32("category_id");
				selectTemp.Close();
			}

			return parentCategoryId;
		}

		private static string GetManufacturer(string temp)
		{
			const string startManufacturerRegex = "\\<a href=\"m(\\d+)/(\\w+).(html)\"\\>\r\n.*";
			const string manufacturerRegex = "title=\"\\w+\"";

			var manufacturerForProductTemp = new Regex(manufacturerRegex).Match(new Regex(startManufacturerRegex).Match(temp).Value).Value;
			return String.IsNullOrEmpty(manufacturerForProductTemp) ? null : manufacturerForProductTemp.Split('"')[1];
		}
		
		private static List<string> GetCategories(string temp)
		{
			const string startCategoryRegex = "class=\"headerNavigation\">";
			const string categoryRegex = "(\\w+|(\\w+-*)*|(\\w+\\s*)*)";
			const string endCategoryRegex = "\\</a\\>";
			const string categoriesForTrash = "(Start|Katalog|\\d+)";
			var categoriesForProduct = new List<string>();

			var categoriesForProductTemp = new Regex(startCategoryRegex + categoryRegex + endCategoryRegex).Matches(temp);
			foreach (Match category in categoriesForProductTemp)
			{
				var categoriesStartTemp = new Regex(startCategoryRegex).Match(category.Value).Length;
				var categoriesEndTemp = new Regex(endCategoryRegex).Match(category.Value).Index - categoriesStartTemp;
				var tempCategory = category.Value.Substring(categoriesStartTemp, categoriesEndTemp);
				if (!new Regex(categoriesForTrash).Match(tempCategory).Success)
					categoriesForProduct.Add(tempCategory);
			}

			return categoriesForProduct;
		}

		private static List<string> GetSubProductsDescr(string temp)
		{
			const string startSubProductsRegex = "\\<td class=\"slaveProductsContent\"\\>\\<a href=\".*\"\\>";
			const string allCharactersInLineRegex = ".*";
			const string endSubProductsRegex = "\\</a\\>\\</td\\>";
			var subProductsOfMainProduct = new List<string>();

			var subProductsOfMainProductTemp = new Regex(startSubProductsRegex + allCharactersInLineRegex).Matches(temp);
			foreach (Match subProduct in subProductsOfMainProductTemp)
			{
				var subProductStartTemp = new Regex(startSubProductsRegex).Match(subProduct.Value).Length;
				var subProductEndTemp = new Regex(endSubProductsRegex).Match(subProduct.Value).Index - subProductStartTemp;
				subProductsOfMainProduct.Add(subProduct.Value.Substring(subProductStartTemp, subProductEndTemp));
			}

			//porządki 1

			for (var i = 0; i < subProductsOfMainProduct.Count; i++)
			{
				const string clearingChars = "'";
				subProductsOfMainProduct[i] = new Regex(clearingChars).Replace(subProductsOfMainProduct[i], "");

				var lengthOfSubProducts = subProductsOfMainProduct[i].Length - 1;
				if (subProductsOfMainProduct[i][lengthOfSubProducts] == ' ' || subProductsOfMainProduct[i][lengthOfSubProducts] == '-')
				{
					subProductsOfMainProduct[i] = subProductsOfMainProduct[i].Remove(lengthOfSubProducts);
				}
			}

			return subProductsOfMainProduct;
		}

		private static List<string> GetProductNumbers(string temp)
		{
			const string startNumbersOfMainProductRegex = "\\<td class=\"slaveProductsContent\">";
			const string digitRegex = "\\d+";
			var numbersOfMainProduct = new List<string>();

			var numbersOfMainProductTemp = new Regex(startNumbersOfMainProductRegex + digitRegex).Matches(temp);
			foreach (Match number in numbersOfMainProductTemp)
			{
				var numberLengthTemp = new Regex(startNumbersOfMainProductRegex).Match(number.Value).Length;
				numbersOfMainProduct.Add(number.Value.Substring(numberLengthTemp));
			}

			return numbersOfMainProduct;
		}

		private static string GetDescription(string temp)
		{
			const string startMainDescrRegex = "\\<td .* class=\"mainDescription\"\\>";
			const string allCharacteresRegex = "((.*(\n)*)*)";
			const string endMainDescrRegex = "\\</td\\>";

			//wycina tymczasowo cały od rozpoczęcia regexu
			var productMainDescrTemp = new Regex(startMainDescrRegex + allCharacteresRegex).Match(temp).Value;

			//wstępne porządki
			const string checkRegex = "table";
			var checkRegexTemp = new Regex(checkRegex).Match(productMainDescrTemp);
			if (checkRegexTemp.Success)
			{
				const string cleanRegex = "\\<table.*\r\n\\s*.*\r\n\\s*.*\r\n\\s*.*\\</table\\>\\<br\\>";
				productMainDescrTemp = new Regex(cleanRegex).Replace(productMainDescrTemp, "");
			}

			//zwraca ile znaków zajmuje startMainDescrRegex
			//+ 2 - usunięcie \r\n z początku napisu
			var productMainDescrStartLocation = new Regex(startMainDescrRegex).Match(productMainDescrTemp).Length + 2;

			//zwraca początek </td>
			//- productmainDescrEndLocation - w substring podaje się ile znaków ma się wyciąć, a nie - końcowy indeks
			var productMainDescrEndLocation = new Regex(endMainDescrRegex).Match(productMainDescrTemp).Index - productMainDescrStartLocation;

			productMainDescrTemp = productMainDescrTemp.Substring(productMainDescrStartLocation, productMainDescrEndLocation);

			//porządki
			const string clearingChars = "'";
			productMainDescrTemp = new Regex(clearingChars).Replace(productMainDescrTemp, "");

			//porządki 2
			const string clearRegex = "\\<br\\>\\<br\\>\\<br\\>";
			var clearRegexTemp = new Regex(clearRegex).Match(productMainDescrTemp).Index;

			if (clearRegexTemp != 0)
				productMainDescrTemp = productMainDescrTemp.Remove(clearRegexTemp);

			//porządki 3
			const string clearRegex2 = "\\<br\\>\\<br\\>\r\n\\s+\\<font";
			var clearRegex2Temp = new Regex(clearRegex2).Match(productMainDescrTemp).Index;

			if (clearRegex2Temp != 0)
				productMainDescrTemp = productMainDescrTemp.Remove(clearRegex2Temp);

			//porządki 4
			const string clearRegex3 = "\\<br\\>\r\n\\s*\\<br\\>\\<img src=\"http://www.*";
			productMainDescrTemp = new Regex(clearRegex3).Replace(productMainDescrTemp, "");

			//porządki 5
			const string clearRegex4 = "\\<br\\>.*sklep@moto-akcesoria\\.pl.*\r\n\\s*.*";
			productMainDescrTemp = new Regex(clearRegex4).Replace(productMainDescrTemp, "");

			//porządki 6
			const string clearRegex5 = "\\<br\\>\r\n\\s*" + clearRegex4;
			productMainDescrTemp = new Regex(clearRegex5).Replace(productMainDescrTemp, "");

			return productMainDescrTemp;
		}
	}
}
