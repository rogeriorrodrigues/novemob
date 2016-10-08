using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using Xamarin.Forms;

//Usado para geolocalização
using Xamarin.Forms.Maps;
using Plugin.Geolocator;




namespace novemob
{

	public partial class EnderecoPage : ContentPage
	{
		Geocoder geoCoder;
		
		public EnderecoPage()
		{
			InitializeComponent();

			geoCoder = new Geocoder();
			//geoloc();
		}

		async void Handle_Unfocused(object sender, Xamarin.Forms.FocusEventArgs e)
		{


			//transformar endereço em lat e long
			var localizacaoPossivel = await geoCoder.GetPositionsForAddressAsync(txtEndereco.Text);

			//Pegar a lat e long possivel
			foreach (var posicao in localizacaoPossivel)
			{

				lblLat.Text += posicao.Latitude;
				lblLong.Text += posicao.Longitude;
			}

			//URL de servico free sobre clima
			string url = "http://api.geonames.org/findNearByWeatherJSON?lat={0}&lng={1}&username=deznetfiap";

			//criando requisição rest
			HttpClient cliente = new HttpClient();

			//string com os parametros para a URL
			var uri = new Uri(string.Format(url, new object[] { lblLat.Text, lblLong.Text}));

			//Fazendo um GET no serviço
			var response = await cliente.GetAsync(uri);

			//Classe para deserializar o retorno
			TempoResultModel tempo = new TempoResultModel();

			//verificando se o status code é OK
			if (response.IsSuccessStatusCode)
			{
				//pegando o retorno de lendo em async
				var content = await response.Content.ReadAsStringAsync();

				//deserializando o conteudo para a classe result
				tempo = JsonConvert.DeserializeObject<TempoResultModel>(content);

				// Apresentando os dados nas labels
				lblTemp.Text = tempo.weatherObservation.temperature;
				lblLoc.Text = tempo.weatherObservation.stationName;


			}

			//Centralizando mapa na localização
			map.MoveToRegion(MapSpan.FromCenterAndRadius(
				new Position(double.Parse(lblLat.Text), double.Parse(lblLong.Text)),
				Distance.FromMiles(1)));

			//Criando Pin
			var pin = new Pin
			{
				Type = PinType.Place,
				Position = new Position(double.Parse(lblLat.Text), double.Parse(lblLong.Text)),
				Label = "Minha Localização",
				Address = txtEndereco.Text
			};

			//Add Pin ao mapa
			map.Pins.Add(pin);

		}

		async void geoloc()
		{
			//Pegar dados atuais de geolocation
			var locator = CrossGeolocator.Current;
			locator.DesiredAccuracy = 100;

			var position = await locator.GetPositionAsync(1000);

			string latitude = position.Latitude.ToString();
			string longitude = position.Longitude.ToString();


			//transformar lat e long em endereço
			var enderecoPossivel = await geoCoder.GetAddressesForPositionAsync(
				new Position(position.Latitude, position.Longitude));

			//Pegar o endereço possivel e exibir na label
			foreach (var endereco in enderecoPossivel)
			{
				lblEndereco.Text += endereco + "\n";
			}

			//Centralizando mapa na localização
			map.MoveToRegion(MapSpan.FromCenterAndRadius(
				new Position(position.Latitude, position.Longitude), 
				Distance.FromMiles(1)));
			
			//Criando Pin
			var pin = new Pin { 
				Type = PinType.Place,
				Position = new Position(position.Latitude, position.Longitude),
				Label = "Minha Localização",
				Address = lblEndereco.Text
			};

			//Add Pin ao mapa
			map.Pins.Add(pin);




			lblLat.Text = latitude;
			lblLong.Text = longitude;

			
		}

		async void buscaCep(object sender, Xamarin.Forms.FocusEventArgs e)
		{
			string sUrl = "https://viacep.com.br/ws/{0}/json/";

			HttpClient client = new HttpClient();


			var uri = new Uri(string.Format(sUrl, txtCep.Text));

			var response = await client.GetAsync(uri);

			CepResult cep = new CepResult();

			if (response.IsSuccessStatusCode)
			{
				var content = await response.Content.ReadAsStringAsync();

				cep = JsonConvert.DeserializeObject<CepResult>(content);

				txtRua.Text = cep.rua;
				txtCidade.Text = cep.cidade;
				txtUF.Text = cep.uf;
				txtBairro.Text = cep.bairro;

				txtNumero.Focus();

			}


		}
	}
}
