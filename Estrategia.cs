
using System;
using System.Collections.Generic;
namespace DeepSpace
{

	class Estrategia
	{
		
		
		public String Consulta1(ArbolGeneral<Planeta> arbol)
		{
			Cola<ArbolGeneral<Planeta>> cola = new Cola<ArbolGeneral<Planeta>>();
			ArbolGeneral<Planeta> Arbolaux;
			
			int distancia = 0;
			cola.encolar(arbol);
			cola.encolar(null);
			
			while (!cola.esVacia()){
				Arbolaux = cola.desencolar();
				if(Arbolaux == null){
					distancia++;
					if(!cola.esVacia()){
						cola.encolar(null);
					}
				}
				else{
					foreach (var hijos in Arbolaux.getHijos()){
						cola.encolar(hijos);
						
					}
					if(Arbolaux.getDatoRaiz().EsPlanetaDeLaIA()){
						
						return "El bot se encuentra a una distancia de " + distancia + " de la raiz";
						
					}
					
				}
				
				
			}
			
			return "algo salio mal";
			
		}


		public String Consulta2(ArbolGeneral<Planeta> arbol)
		{
 


			String respuesta = " ";
			ArbolGeneral<Planeta> aux = null;
			Cola<ArbolGeneral<Planeta>> cola = new Cola<ArbolGeneral<Planeta>>();
			cola.encolar(arbol);  //raiz
			while (!cola.esVacia()) {
				aux = cola.desencolar();  //desencola el primer elemento

				if (aux.getDatoRaiz().EsPlanetaDeLaIA()) {         //caso 1: la raiz es el bot
					if (!aux.esHoja()) {
						foreach (var hijo in aux.getHijos()) {
							respuesta += " " + preordenMain(hijo, respuesta);
						}
					} else {
						respuesta = "es hoja";
					}
             
					break;
				} else {
					foreach (var hijo in aux.getHijos()) {
						cola.encolar(hijo);
					}


				}
			}

			return "Descendientes del planeta del bot : " + respuesta;

		}

		public String preordenMain(ArbolGeneral<Planeta> arbol, string respuesta)
		{
			respuesta = (arbol.getDatoRaiz().Poblacion()).ToString();
			List<ArbolGeneral<Planeta>> listaHijos = arbol.getHijos();
			foreach (var hijo in listaHijos) {
				respuesta += " " + preordenMain(hijo, respuesta) + "/";
			}
			return respuesta;
		}




		public String Consulta3(ArbolGeneral<Planeta> arbol)
		{
			String respuesta = "";
			int poblacionTotal = 0;
			int Planetaspornivel = 0;
			int nivel = 0;
			int promedio = 0;
			ArbolGeneral<Planeta> aux = null;
			Cola<ArbolGeneral<Planeta>> cola = new Cola<ArbolGeneral<Planeta>>();
			cola.encolar(arbol);  //raiz
			cola.encolar(null);    //separador de nivels
			while (!cola.esVacia()) {
				aux = cola.desencolar();  //desencola el primer elemento

				if (aux != null) {
					Planetaspornivel++;
					poblacionTotal += aux.getDatoRaiz().Poblacion();


					foreach (var hijo in aux.getHijos()) {
						cola.encolar(hijo);

					}
				} else if (!cola.esVacia() || Planetaspornivel != 0) {

					cola.encolar(null);
					promedio = poblacionTotal / Planetaspornivel;

					respuesta += "Nivel " + nivel + " poblacion total " + poblacionTotal + ", promedio " + promedio + "\n";
					poblacionTotal = 0;
					Planetaspornivel = 0;
					nivel++;
				}

			}
			return respuesta;
		}
		
			
public bool caminoJugador(ArbolGeneral<Planeta> arbol, ArbolGeneral<Planeta> origen , List<ArbolGeneral<Planeta>> camino){
	
	bool caminoEncontrado = false;
	
	camino.Add(origen);
	
	if(origen.getDatoRaiz().EsPlanetaDelJugador()){
		caminoEncontrado = true;
	}
	else{
		foreach(var hijo in origen.getHijos()){
			caminoEncontrado = caminoJugador(arbol,hijo,camino);
				if(caminoEncontrado){
				break;
			}	
		camino.RemoveAt(camino.Count-1);			
		}
		
	}
	return caminoEncontrado;
}
public bool caminoBot(ArbolGeneral<Planeta> arbol, ArbolGeneral<Planeta> origen , List<ArbolGeneral<Planeta>> camino){
	
	bool caminoEncontrado = false;
	
	camino.Add(origen);
	
	if(origen.getDatoRaiz().EsPlanetaDeLaIA()){
		caminoEncontrado = true;
	}
	else{
		foreach(var hijo in origen.getHijos()){
			caminoEncontrado = caminoBot(arbol,hijo,camino);
				if(caminoEncontrado){
				break;
			}		
			camino.RemoveAt(camino.Count-1);
		}
		
	}
	
	return caminoEncontrado;
}
		
		public Movimiento CalcularMovimiento(ArbolGeneral<Planeta> arbol){	
			
			 List<ArbolGeneral<Planeta>> jugador = new List<ArbolGeneral<Planeta>>();
			 List<ArbolGeneral<Planeta>> bot = new List<ArbolGeneral<Planeta>>();
			 
			 //Armo las listas del bot y jugador
			 caminoJugador(arbol,arbol,bot);
			 caminoBot(arbol,arbol,bot);
			 
			 //Evaluo si la lista del bot en la pos 0 no es planeta de la IA, y en ese caso el movimiento es desde el bot hacia el planeta vecino
    		 
    		if(!bot[0].getDatoRaiz().EsPlanetaDeLaIA()){ 
    			
    			Movimiento movimiento = new Movimiento(bot[bot.Count-1].getDatoRaiz(),bot[bot.Count-2].getDatoRaiz());
    			return movimiento; 
    		}
 
    		int contador = 0;
    		//Si el planeta de la pos 0 de la lista de jugador es planeta de la ia, entonces, mientras que el contador sea menor que los elementos de la lista de jugador
    		//Entonces, si la lista del jugador en la posicion del contador es planeta de la ia y el planeta vecino no es planeta de la ia, netonces el movimiento
    		//seria desde la posicion del contador y la pos del contador mas 1. 
    		if(jugador[0].getDatoRaiz().EsPlanetaDeLaIA()){
    			while(contador < jugador.Count){
    				if(jugador[contador].getDatoRaiz().EsPlanetaDeLaIA() && !jugador[contador+1].getDatoRaiz().EsPlanetaDeLaIA()){
    					Movimiento movimiento = new Movimiento(jugador[contador].getDatoRaiz(),jugador[contador+1].getDatoRaiz());
    						return movimiento;
    				}
    				
    				else{
    					contador++;
    				}
    			}
    		}
    		
			return null;
		}
		
		public ArbolGeneral<Planeta> BuscarplanetaIA (ArbolGeneral<Planeta> planeta){
			
			if(planeta.getDatoRaiz().EsPlanetaDeLaIA()){
				return planeta;
			}
			foreach(var hijo in planeta.getHijos()){
				ArbolGeneral<Planeta> resultado = BuscarplanetaIA(hijo);
				if (resultado != null){
					return resultado;
				}
					
			}
			return null;
		}
		public ArbolGeneral<Planeta> BuscarplanetaUsuario(ArbolGeneral<Planeta> planeta){
			
			if(planeta.getDatoRaiz().EsPlanetaDelJugador()){
				return planeta;
			}
			foreach(var hijo in planeta.getHijos()){
				ArbolGeneral<Planeta> resultado = BuscarplanetaUsuario(hijo);
				if (resultado != null){
					return resultado;
				}
					
			}
			return null;
		}
  }	  
		
		
}
	
		
