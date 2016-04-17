using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.PM;
using Android.Media;

namespace JogoDaVelhaNicolasViana
{
	[Activity (Label = "JogoDaVelhaNicolasViana", MainLauncher = true, Icon = "@drawable/icon", 
		ScreenOrientation = ScreenOrientation.Landscape, Theme = "@android:style/Theme.Holo.NoActionBar.Fullscreen")]

	public class MainActivity : Activity
	{
		static MediaPlayer vitoriaVader, vitoriaLuke;
		static int[,] c;
		static Button[,] b;
		static int i, j = 0, pontLuke=0, pontVader=0;
		static TextView textView, textViewPontuacao;
		static CPU cpu;

		protected override void OnCreate (Bundle bundle)
		{
			vitoriaVader = MediaPlayer.Create(this, Resource.Raw.vader);
			vitoriaLuke = MediaPlayer.Create(this, Resource.Raw.luke);
			base.OnCreate (bundle);
			SetContentView(Resource.Layout.Main);
			Inicializar();
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.OptionsMenu, menu);
			return base.OnCreateOptionsMenu(menu);
		}

		public override bool OnOptionsItemSelected(IMenuItem item) 
		{
			switch (item.ItemId)
			{
			case Resource.Id.new_game:
				{
					MainActivity.HabilitaQuadrados ();
					vitoriaVader.Stop ();
					vitoriaLuke.Stop ();
					Inicializar();
					return true;
				}
			}
			return true;
		}

		private void Inicializar() 
		{
			cpu = new CPU();
			c = new int[4,4];
			b  = new Button[4,4];
			textView = (TextView) FindViewById(Resource.Id.dialogue);
			textViewPontuacao = (TextView)FindViewById (Resource.Id.pontuacao);
			b[1,3] = (Button) FindViewById(Resource.Id.one);
			b[1,2] = (Button) FindViewById(Resource.Id.two);
			b[1,1] = (Button) FindViewById(Resource.Id.three);
			b[2,3] = (Button) FindViewById(Resource.Id.four);
			b[2,2] = (Button) FindViewById(Resource.Id.five);
			b[2,1] = (Button) FindViewById(Resource.Id.six);
			b[3,3] = (Button) FindViewById(Resource.Id.seven);
			b[3,2] = (Button) FindViewById(Resource.Id.eight);
			b[3,1] = (Button) FindViewById(Resource.Id.nine);
			for (i = 1; i <= 3; i++) 
			{
				for (j = 1; j <= 3; j++)
					c[i,j] = 2;
			}
			textView.Text = "Clique num botão para começar!";
			for (i = 1; i <= 3; i++) 
			{
				for (j = 1; j <= 3; j++) 
				{
					b[i,j].SetOnClickListener(new MyClickListener(i, j, b, c, textView, 
																	pontLuke, pontVader, textViewPontuacao, 
																	vitoriaVader, vitoriaLuke));
					if(!b[i,j].Enabled==true) 
					{
						b[i,j].Text=" ";
						b[i,j].Enabled=true;
					}
				}
			}
		}

		class MyClickListener: Java.Lang.Object, View.IOnClickListener 
		{
			int x;
			int y;
			Button[,] b;
			int[,] c;
			MediaPlayer vitoriaVader;
			MediaPlayer vitoriaLuke;
			TextView textView;
			TextView textViewPontuacao;
			int pontLuke;
			int pontVader;
			public MyClickListener(int x, int y, Button[,] b, int[,] c, TextView textView, 
									int pontLuke, int pontVader, TextView textViewPontuacao, 
									MediaPlayer vitoriaVader,MediaPlayer vitoriaLuke) 
			{
				this.x = x;
				this.y = y;
				this.b = b;
				this.c = c;
				this.vitoriaVader = vitoriaVader;
				this.vitoriaLuke = vitoriaLuke;
				this.textView = textView;
				this.pontLuke=pontLuke;
				this.pontVader=pontVader;
				this.textViewPontuacao=textViewPontuacao;
			}

			public void OnClick(View view) 
			{
				if (b[x,y].Enabled==true) 
				{
					b[x,y].Enabled=false;
					b[x,y].Text="O";
					c[x,y] = 0;
					textView.Text = "";
					if (!MainActivity.tabuleiroJogo()) 
					{
						cpu.TurnoJogador();
					}
				}
			}
		}

		private class CPU 
		{
			public void TurnoJogador() 
			{
				if(c[1,1]==2 &&
					((c[1,2]==0 && c[1,3]==0) ||
						(c[2,2]==0 && c[3,3]==0) ||
						(c[2,1]==0 && c[3,1]==0))) 
				{
					MarcarQuadrado(1,1);
				} 
				else if (c[1,2]==2 &&
					((c[2,2]==0 && c[3,2]==0) ||
						(c[1,1]==0 && c[1,3]==0))) 
				{
					MarcarQuadrado(1,2);
				} 
				else if(c[1,3]==2 &&
					((c[1,1]==0 && c[1,2]==0) ||
						(c[3,1]==0 && c[2,2]==0) ||
						(c[2,3]==0 && c[3,3]==0))) 
				{
					MarcarQuadrado(1,3);
				} 
				else if(c[2,1]==2 &&
					((c[2,2]==0 && c[2,3]==0) ||
						(c[1,1]==0 && c[3,1]==0)))
				{
					MarcarQuadrado(2,1);
				} 
				else if(c[2,2]==2 &&
					((c[1,1]==0 && c[3,3]==0) ||
						(c[1,2]==0 && c[3,2]==0) ||
						(c[3,1]==0 && c[1,3]==0) ||
						(c[2,1]==0 && c[2,3]==0))) 
				{
					MarcarQuadrado(2,2);
				} 
				else if(c[2,3]==2 &&
					((c[2,1]==0 && c[2,2]==0) ||
						(c[1,3]==0 && c[3,3]==0))) 
				{
					MarcarQuadrado(2,3);
				} 
				else if(c[3,1]==2 &&
					((c[1,1]==0 && c[2,1]==0) ||
						(c[3,2]==0 && c[3,3]==0) ||
						(c[2,2]==0 && c[1,3]==0)))
				{
					MarcarQuadrado(3,1);
				} 
				else if(c[3,2]==2 &&
					((c[1,2]==0 && c[2,2]==0) ||
						(c[3,1]==0 && c[3,3]==0))) 
				{
					MarcarQuadrado(3,2);
				}
				else if( c[3,3]==2 &&
					((c[1,1]==0 && c[2,2]==0) ||
						(c[1,3]==0 && c[2,3]==0) ||
						(c[3,1]==0 && c[3,2]==0))) 
				{
					MarcarQuadrado(3,3);
				} 
				else 
				{
					Random rand = new Random();

					int a = rand.Next(4);
					int b = rand.Next(4);
					while(a==0 || b==0 || c[a,b]!=2) {
						a = rand.Next(4);
						b = rand.Next(4);
					}
					CPU.MarcarQuadrado(a,b);
				}
			}

			private static void MarcarQuadrado(int x, int y) 
			{
				b[x,y].Enabled=false;
				b[x,y].Text="X";
				c[x,y] = 1;
				MainActivity.tabuleiroJogo();
			}
		}

		private static void HabilitaQuadrados()
		{
			int x;
			for (x= 1; x<=3; x++) 
			{
				b[x,x].Enabled = true;
				b[x,x].Text = "";
			}
		}
		private static void DesabilitaQuadrados()
		{
			b[1,3].Enabled = false;
			b[1,2].Enabled = false;
			b[1,1].Enabled = false;
			b[2,3].Enabled = false;
			b[2,2].Enabled = false;
			b[2,1].Enabled = false;
			b[3,3].Enabled = false;
			b[3,2].Enabled = false;
			b[3,1].Enabled = false;
		}
		private static bool tabuleiroJogo() 
		{
			bool fimJogo = false;
			if ((c[1,1] == 0 && c[2,2] == 0 && c[3,3] == 0)
				|| (c[1,3] == 0 && c[2,2] == 0 && c[3,1] == 0)
				|| (c[1,2] == 0 && c[2,2] == 0 && c[3,2] == 0)
				|| (c[1,3] == 0 && c[2,3] == 0 && c[3,3] == 0)
				|| (c[1,1] == 0 && c[1,2] == 0 && c[1,3] == 0)
				|| (c[2,1] == 0 && c[2,2] == 0 && c[2,3] == 0)
				|| (c[3,1] == 0 && c[3,2] == 0 && c[3,3] == 0)
				|| (c[1,1] == 0 && c[2,1] == 0 && c[3,1] == 0)) 
			{
				textView.Text="Fim de batalha. \n Vitória Jedi!";
				try 
				{
					vitoriaLuke.PrepareAsync();
					vitoriaLuke.Start();
				} 
				catch (Exception) 
				{

				}
				pontLuke++;
				textViewPontuacao.Text = "Luke " +pontLuke.ToString()+ " X " + pontVader.ToString()+ " Vader";
				fimJogo = true;
				MainActivity.DesabilitaQuadrados ();
			} 
			else if ((c[1,1] == 1 && c[2,2] == 1 && c[3,3] == 1)
				|| (c[1,3] == 1 && c[2,2] == 1 && c[3,1] == 1)
				|| (c[1,2] == 1 && c[2,2] == 1 && c[3,2] == 1)
				|| (c[1,3] == 1 && c[2,3] == 1 && c[3,3] == 1)
				|| (c[1,1] == 1 && c[1,2] == 1 && c[1,3] == 1)
				|| (c[2,1] == 1 && c[2,2] == 1 && c[2,3] == 1)
				|| (c[3,1] == 1 && c[3,2] == 1 && c[3,3] == 1)
				|| (c[1,1] == 1 && c[2,1] == 1 && c[3,1] == 1)) 
			{
				textView.Text="Fim de batalha. \n Vitória do Império!";
				try 
				{
					vitoriaVader.Prepare();
					vitoriaVader.Start();
				} 
				catch (Exception) 
				{

				}
				pontVader++;
				textViewPontuacao.Text = "Luke " +pontLuke.ToString()+ " X " + pontVader.ToString()+ " Vader";
				fimJogo = true;
				MainActivity.DesabilitaQuadrados ();
			} 
			else 
			{
				bool vazio = false;
				for(i=1; i<=3; i++) 
				{
					for(j=1; j<=3; j++) 
					{
						if(c[i,j]==2) 
						{
							vazio = true;
							break;
						}
					}
				}
				if(!vazio) 
				{
					fimJogo = true;
					textView.Text = "Fim de batalha. Empate!";
					MainActivity.DesabilitaQuadrados ();
				}
			}
			return fimJogo;
		}
	}
}

