using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Threading;
using System.Diagnostics;
using Community.CsharpSqlite;


public class DatabaseHelper{

	/// <summary>
	/// The database filename.
	/// </summary>
	public static string dataBaseFilename = "game.db";


	private static bool databaseCreated = false;

	/// <summary>
	/// guarda la instancia de la base de datos
	/// </summary>
	private static SQLiteDB db = null; 
	private static SQLiteQuery query;



	//TODO: descomentar en produccion
	private static string queryKey = "PRAGMA key='0x2153913b4864103f710a0b0c0cd45f10';";

    public static void CreateTables()
    {
        // Created by Vertabelo (http://vertabelo.com)
        // Script type: create
        // Scope: [tables, references, sequences, views, procedures]
        // Generated at Thu Feb 12 04:43:22 UTC 2015
        string sqlCreate = @"CREATE TABLE Dificultades (
                id integer NOT NULL  PRIMARY KEY AUTOINCREMENT,
                nombre varchar(25) NOT NULL
            );

            CREATE TABLE Letras (
                id integer NOT NULL  PRIMARY KEY AUTOINCREMENT,
                letra character(1) NOT NULL,
                dificultad integer NOT NULL,
                FOREIGN KEY (dificultad) REFERENCES Dificultades (id)
            );

            CREATE TABLE Niveles (
                id integer NOT NULL  PRIMARY KEY AUTOINCREMENT,
                nombre varchar(25) NOT NULL,
                etapas integer NOT NULL,
                dificultad integer NOT NULL
            );

            CREATE TABLE NivelesLetras (
                id integer NOT NULL  PRIMARY KEY AUTOINCREMENT,
                nivel integer NOT NULL,
                letra integer NOT NULL,
                FOREIGN KEY (nivel) REFERENCES Niveles (id),
                FOREIGN KEY (letra) REFERENCES Letras (id)
            );
            CREATE TABLE NivelesPalabras (
                id integer NOT NULL  PRIMARY KEY AUTOINCREMENT,
                nivel integer NOT NULL,
                palabra integer NOT NULL,
                FOREIGN KEY (nivel) REFERENCES Niveles (id),
                FOREIGN KEY (palabra) REFERENCES Palabras (id)
            );
            CREATE TABLE NivelesSilabas (
                id integer NOT NULL  PRIMARY KEY AUTOINCREMENT,
                nivel integer NOT NULL,
                silaba integer NOT NULL,
                FOREIGN KEY (nivel) REFERENCES Niveles (id),
                FOREIGN KEY (silaba) REFERENCES Silabas (id)
            );
            CREATE TABLE Palabras (
                id integer NOT NULL  PRIMARY KEY AUTOINCREMENT,
                palabra varchar(255) NOT NULL,
                dificultad integer NOT NULL,
                FOREIGN KEY (dificultad) REFERENCES Dificultades (id)
            );
            CREATE TABLE PalabrasSilabas (
                id integer NOT NULL  PRIMARY KEY AUTOINCREMENT,
                palabra integer NOT NULL,
                silabaprev integer NOT NULL,
                silabanext integer NOT NULL,
                FOREIGN KEY (silabaprev) REFERENCES Silabas (id),
                FOREIGN KEY (palabra) REFERENCES Palabras (id),
                FOREIGN KEY (silabanext) REFERENCES Silabas (id)
            );
            CREATE TABLE Partidas (
                id integer NOT NULL  PRIMARY KEY AUTOINCREMENT,
                perfil integer NOT NULL,
                nivel integer NOT NULL,
                ejercicio varchar(50) NOT NULL,
                actividad varchar(50) NOT NULL,
                fechaInicio datetime NOT NULL,
                fechaFin datetime NOT NULL,
                FOREIGN KEY (nivel) REFERENCES Niveles (id),
                FOREIGN KEY (perfil) REFERENCES Perfiles (id)
            );
            CREATE TABLE Perfiles (
                id integer NOT NULL  PRIMARY KEY AUTOINCREMENT,
                nombre varchar(50) NOT NULL
            );
            CREATE TABLE Puntajes (
                id integer NOT NULL  PRIMARY KEY AUTOINCREMENT,
                perfil integer NOT NULL,
                actividad varchar(50) NOT NULL,
                puntaje integer NOT NULL,
                FOREIGN KEY (perfil) REFERENCES Perfiles (id)
            );
            CREATE TABLE Silabas (
                id integer NOT NULL  PRIMARY KEY AUTOINCREMENT,
                silaba varchar(10) NOT NULL,
                dificultad integer NOT NULL,
                FOREIGN KEY (dificultad) REFERENCES Dificultades (id)
            );
            CREATE TABLE Stats (
                id integer NOT NULL  PRIMARY KEY AUTOINCREMENT,
                partida integer NOT NULL,
                etapa integer NOT NULL,
                correcto boolean NOT NULL,
                fechaInicio datetime NOT NULL,
                fechaFin datetime NOT NULL,
                fechaPregunta datetime NOT NULL,
                fechaRespuesta datetime NOT NULL,
                FOREIGN KEY (partida) REFERENCES Partidas (id)
            );"
    }

    public static void DropTables()
    {
        
    }
	public static void createDatabase(){
		if (!databaseCreated) {
			databaseCreated = true;
			OpenDataBase();
			SQLiteQuery qr = null;						
			try{

				qr = new SQLiteQuery(db,queryProducts);
				qr.Step();
				qr.Release();
				qr = null;


			}catch(Exception e)
			{
				UnityEngine.Debug.LogError("ERROR AL CREAR TABLAS: "+e.Message);
			}

			qr = new SQLiteQuery(db,"SELECT count(*) as total FROM power_ups WHERE id = 'TIM';");

			int exist = 0;
			while(qr.Step())
			{
				exist = qr.GetInteger("total");
			}
			qr.Release();

			if(exist == 0)
			{
				try{
					
					qr = new SQLiteQuery(db,"INSERT INTO power_ups VALUES('TIM',10);");
					qr.Step();
					qr.Release();
					
					qr = new SQLiteQuery(db,"INSERT INTO power_ups VALUES('DUP',10);");
					qr.Step();
					qr.Release();
					
					qr = new SQLiteQuery(db,"INSERT INTO power_ups VALUES('REM',10);");
					qr.Step();
					qr.Release();

					qr = new SQLiteQuery(db,"INSERT INTO feats VALUES('AC5','5 right answers in a row', 'ico_logro_x5_of','ico_logro_x5_on', NULL, 0);");
					qr.Step();
					qr.Release();

					qr = new SQLiteQuery(db,"INSERT INTO feats VALUES('AC15','15 right answers in a row', 'ico_logro_x15_of','ico_logro_x15_on', NULL, 0);");
					qr.Step();
					qr.Release();

					qr = new SQLiteQuery(db,"INSERT INTO feats VALUES('LC1','Complete a level', 'ico_logro_nivel_completo_of','ico_logro_nivel_completo_on', NULL, 0);");
					qr.Step();
					qr.Release();

					qr = new SQLiteQuery(db,"INSERT INTO feats VALUES('LCWE','Complete a perfect level. Non mistakes', 'ico_logro_nivel_completo_perfecto_of','ico_logro_nivel_completo_perfecto_on', NULL, 0);");
					qr.Step();
					qr.Release();

					qr = new SQLiteQuery(db,"INSERT INTO feats VALUES('UPUT','Use 10 sec. power up', 'ico_logro_power_up_10seg_of','ico_logro_power_up_10seg_on', NULL, 0);");
					qr.Step();
					qr.Release();

					qr = new SQLiteQuery(db,"INSERT INTO feats VALUES('UPUQ','Use less options power up', 'ico_logro_power_up_opciones_of','ico_logro_power_up_opciones_on', NULL, 0);");
					qr.Step();
					qr.Release();

					qr = new SQLiteQuery(db,"INSERT INTO feats VALUES('UPUD','Use doble score power up', 'ico_logro_power_up_doble_puntos_of','ico_logro_power_up_doble_puntos_on', NULL, 0);");
					qr.Step();
					qr.Release();

					qr = new SQLiteQuery(db,"INSERT INTO feats VALUES('PC1','Complete a Galactic ride', 'ico_logro_paseo_completo_of','ico_logro_paseo_completo_on', NULL, 0);");
					qr.Step();
					qr.Release();

					qr = new SQLiteQuery(db,"INSERT INTO feats VALUES('PCG1','Complete the General knowledge galaxy', 'ico_logro_paseo_general_of','ico_logro_paseo_general_on', 2218, 0);");
					qr.Step();
					qr.Release();

					qr = new SQLiteQuery(db,"INSERT INTO feats VALUES('PCGAN1','Complete the Nature and Science galaxy', 'ico_logro_paseo_ciencia_of','ico_logro_paseo_ciencia_on', 710, 0);");
					qr.Step();
					qr.Release();

					qr = new SQLiteQuery(db,"INSERT INTO feats VALUES('PCAAS1','Complete the Art and Show business galaxy', 'ico_logro_paseo_arte_of','ico_logro_paseo_arte_on', 194, 0);");
					qr.Step();
					qr.Release();

					qr = new SQLiteQuery(db,"INSERT INTO feats VALUES('PCSAH1','Complete the Sport and Health galaxy', 'ico_logro_paseo_deporte_of','ico_logro_paseo_deporte_on', 1213, 0);");
					qr.Step();
					qr.Release();

					qr = new SQLiteQuery(db,"INSERT INTO feats VALUES('PCMAC1','Complete the Myths and Curiosities galaxy', 'ico_logro_paseo_mitos_of','ico_logro_paseo_mitos_on', 1723, 0);");
					qr.Step();
					qr.Release();


				}catch(Exception e)
				{
					UnityEngine.Debug.LogError("ERROR AL INSERTAR POWER UPS: "+e.Message);
				}
			}

			CloseDataBase();
		}
	}

	private static bool dbOpened = false;
	/// <summary>
	/// Opens the database.
	/// </summary>
	/// <returns><c>true</c>, if data base was opened, <c>false</c> otherwise.</returns>
	public static bool OpenDataBase()
	{
		if(db == null)
		{
			db = new SQLiteDB();
		}
		if(!dbOpened){
			try
			{

				db.Open(Application.persistentDataPath+"/"+dataBaseFilename);
				db.Key(queryKey);
				dbOpened = true;
			}
			catch(Exception e)
			{
				UnityEngine.Debug.LogError("Open Data Base Error: " + e.ToString());
			}
		}
		return dbOpened;
	}

	/// <summary>
	/// Closes the database.
	/// </summary>
	/// <returns><c>true</c>, if data base was closed, <c>false</c> otherwise.</returns>
	public static bool CloseDataBase()
	{
		bool aux = false;
		dbOpened = false;
		try
		{
			if(db != null)
			{
				db.Close();
				db = null;
			}
			aux = true;
		}
		catch(Exception e)
		{
			UnityEngine.Debug.LogError("Close Data Base Error: " + e.ToString());
		}

		return aux;
	}
    /*
	/// <summary>
	/// add a Product.
	/// </summary>
	/// <returns>The id of add product.</returns>
	/// <param name="product">Product.</param>
	public static int ProductAdd(Product product){

		string queryString = "INSERT INTO products (guid, name, tid, uuid, premium, downloaded, owned, modified) VALUES(?,?,?,?,?,?,?,?)";
		try {
			//save data from package
			SQLiteQuery query = new SQLiteQuery (db, queryString);
			
			query.Bind (product.guid);
			query.Bind (product.name);
			query.Bind (product.category);
			query.Bind (product.uuid);
			query.Bind (product.premium);
			query.Bind (product.downloaded);
			query.Bind (product.owned);
			query.Bind(product.modified);
			query.Step ();
			query.Release ();
			
			queryString = "SELECT MAX(id) as id FROM products"; //query para seleccionar el ultimo id insertado
			
			query = new SQLiteQuery (db, queryString);
			int productId = -1;
			while (query.Step()) {
				productId = query.GetInteger ("id");
			}
			query.Release ();
			return productId;
			
		} catch (Exception e) {
			UnityEngine.Debug.Log ("Error saving data of Package " + e.ToString ());
			return -1;
		}		
	}

	/// <summary>
	/// update a Product.
	/// </summary>
	/// <returns>The updated id.</returns>
	/// <param name="product">Product.</param>
	public static int ProductUpdate(Product product){
			
		string queryString = "UPDATE products SET name='"+product.name+"', tid="+product.category+", premium="+product.premium+", downloaded="+product.downloaded+", owned="+product.owned+", modified="+product.modified+" WHERE guid ="+product.guid.ToString();
		try
		{
			//save data from package
			SQLiteQuery query = new SQLiteQuery(db,queryString);
			query.Step();
			query.Release();
			
			int packageId = product.guid;				
			return packageId;
			
		} 
		catch (Exception e)
		{
			UnityEngine.Debug.Log("Error saving data of Package " + e.ToString());
			return -1;
		}
	}


	/// <summary>
	/// get a list of products.
	/// </summary>
	/// <returns>The products list.</returns>
	public static List<Product> ProductsGet(){
		string selectQueryString = "SELECT * FROM products";
		List<Product> products = new List<Product> ();
		
		try
		{
			SQLiteQuery query = new SQLiteQuery(db,selectQueryString);
			while(query.Step()){
				Product product = new Product();
				product.guid = query.GetInteger("guid");
				product.name = query.GetString("name");
				product.category = query.GetInteger("tid");
				product.uuid = query.GetString("uuid");
				product.premium = query.GetInteger("premium");
				product.downloaded = query.GetInteger("downloaded");
				product.owned = query.GetInteger("owned");
				product.modified = query.GetInteger("modified");
				products.Add(product);
			}
			
			query.Release();
			
			return products;
			
		} catch (Exception e)
		{
			UnityEngine.Debug.Log("Fallo al devolver packages" + e.ToString());
			return products;
		}		
	}

	/// <summary>
	/// get a product.
	/// </summary>
	/// <returns>The product by guid.</returns>
	public static Product ProductGet(int guid){
		string selectQueryString = "SELECT * FROM products WHERE guid="+guid.ToString();
		Product product = new Product ();
		product.guid = -1;
		product.owned = 0;
		product.modified = 0;
		try
		{
			SQLiteQuery query = new SQLiteQuery(db,selectQueryString);
			while(query.Step()){
				product.guid = query.GetInteger("guid");
				product.name = query.GetString("name");
				product.category = query.GetInteger("tid");
				product.uuid = query.GetString("uuid");
				product.premium = query.GetInteger("premium");
				product.downloaded = query.GetInteger("downloaded");
				product.owned = query.GetInteger("owned");
				product.modified = query.GetInteger("modified");
			}
			
			query.Release();
			
			return product;
			
		} catch (Exception e)
		{
			UnityEngine.Debug.Log("Fallo al devolver packages" + e.ToString());
			return product;
		}		
	}

	/// <summary>
	/// get a product.
	/// </summary>
	/// <returns>The product by guid.</returns>
	public static Product ProductGet(string name){
		string selectQueryString = "SELECT * FROM products WHERE name='"+name.ToString()+"'";
		Product product = new Product ();
		product.guid = -1;
		product.owned = 0;
		product.modified = 0;
		try
		{
			SQLiteQuery query = new SQLiteQuery(db,selectQueryString);
			while(query.Step()){
				product.guid = query.GetInteger("guid");
				product.name = query.GetString("name");
				product.category = query.GetInteger("tid");
				product.uuid = query.GetString("uuid");
				product.premium = query.GetInteger("premium");
				product.downloaded = query.GetInteger("downloaded");
				product.owned = query.GetInteger("owned");
				product.modified = query.GetInteger("modified");
			}
			
			query.Release();
			
			return product;
			
		} catch (Exception e)
		{
			UnityEngine.Debug.Log("Fallo al devolver packages" + e.ToString());
			return product;
		}		
	}

	/// <summary>
	/// Add a new Package.
	/// </summary>
	/// <returns>The Package id.</returns>
	/// <param name="package">Package.</param>
	public static int PackageAdd(Package package){
		if (package.delete != 1) {
			string queryString = "INSERT INTO packages (guid, name, lang, generic_id, category, modified, uuid) VALUES(?,?,?,?,?,?,?)";
			try {
				//save data from package
				SQLiteQuery query = new SQLiteQuery (db, queryString);

				query.Bind (package.guid);
				query.Bind (package.name);
				query.Bind (package.lang);
				query.Bind (package.genericId);
				query.Bind (package.category);
				query.Bind (package.modified);
				query.Bind (package.uuid);
				query.Step ();
				query.Release ();

				queryString = "SELECT MAX(id) as id FROM packages"; //query para seleccionar el ultimo id insertado

				query = new SQLiteQuery (db, queryString);
				int packageId = -1;
				while (query.Step()) {
						packageId = query.GetInteger ("id");
				}
				query.Release ();

				if (package.quizzes != null) {
					foreach (Quiz quiz in package.quizzes) {
						if(quiz.delete!= 1)
						{
							QuizAdd (quiz);
						}
					}
				}

				return packageId;
	
			} catch (Exception e) {
				UnityEngine.Debug.Log ("Error saving data of Package " + e.ToString ());
				return -1;
			}
		} else {
			PackageDelete(package);
			return -1;
		}						

	}

	/// <summary>
	/// updates a Package.
	/// </summary>
	/// <returns>The updated id.</returns>
	/// <param name="package">Package.</param>
	public static int PackageUpdate(Package package){
		if (package.delete != 1) {

			string queryString = "UPDATE packages SET name='"+package.name+"', lang='"+package.lang+"', modified="+package.modified.ToString()+", category='"+package.category+"' WHERE guid ="+package.guid.ToString();
			try
			{
				//save data from package
				SQLiteQuery query = new SQLiteQuery(db,queryString);
				query.Step();
				query.Release();

				int packageId = package.guid;

				if(package.quizzes != null)
				{
					foreach(Quiz quiz in package.quizzes)
					{
						if(quiz.delete == 1){//si esta en estado para borrar, la elimina

							int quizDeleted = QuizDelete(quiz);
							if(quizDeleted == -1){
								UnityEngine.Debug.LogError ("Error al eliminar pregunta");
							}
							//UnityEngine.Debug.Log ("ELIMINAR PREGUNTA:"+quiz.question+" DELETED: "+quizDeleted.ToString());
						}else{

							Quiz savedQuiz = QuizGet(quiz.guid);
							//UnityEngine.Debug.Log("Saved QUIZ: "+savedQuiz.question);
							//si existe una pregunta guarada la edito 
							if(savedQuiz.id != -1){
								//si ha sido modificada la guardo
								if(quiz.modified>savedQuiz.modified){
									//UnityEngine.Debug.Log("La pregunta sido modificada guardar: "+quiz.question);
									quiz.answered = savedQuiz.answered;
									quiz.points = savedQuiz.points;
									QuizUpdate(quiz);
								}
								savedQuiz = null;
							}else { //guardo la pregunta nueva
								//UnityEngine.Debug.Log("Agregar una nueva pregunta: "+quiz.question);
								QuizAdd(quiz);
							}
						}
					}
				}
				
				return packageId;
				
			} 
			catch (Exception e)
			{
				UnityEngine.Debug.LogError("Error saving data of Package " + e.ToString());
				return -1;
			}
		}else{
			PackageDelete(package);
			return -1;
		}
			
	}

	/// <summary>
	/// return a list of Packages
	/// </summary>
	/// <returns>a List of Packages.</returns>
	/// <param name="lang">Language.</param>
	public static List<Package> PackagesGet(string lang = "es"){

		string selectQueryString = "SELECT pack.id, pack.guid, pack.name, pack.lang, pack.generic_id, pack.category, pack.modified, pack.uuid,prod.premium, prod.downloaded, prod.owned, prod.owned FROM packages pack INNER JOIN products prod on pack.generic_id = prod.guid WHERE lang='"+lang+"'";
		List<Package> packages = new List<Package> ();

		try
		{
			SQLiteQuery query = new SQLiteQuery(db,selectQueryString);
			while(query.Step()){
				Package package = new Package();
				package.guid = query.GetInteger("guid");
				package.category = Languages.Translate(query.GetString("category"));
				package.name = query.GetString("name");
				package.genericId = query.GetInteger("generic_id");
				package.uuid = query.GetString("uuid");
				package.downloaded = query.GetInteger("downloaded");
				package.owned = query.GetInteger("owned");
				package.premium = query.GetInteger("premium");
				packages.Add(package);
			}

			query.Release();
			
			return packages;
			
		} catch (Exception e)
		{
			UnityEngine.Debug.LogError("Fallo al devolver packages" + e.ToString());
			return packages;
		}

	}


	public static Package PackageGet(int guid){
		string selectQueryString = "SELECT * FROM packages WHERE guid="+guid.ToString();
		Package package = new Package();
		package.guid = -1;
		
		try
		{
			SQLiteQuery query = new SQLiteQuery(db,selectQueryString);
			while(query.Step()){
				package.guid = query.GetInteger("guid");
				package.category = Languages.Translate(query.GetString("category"));
				package.name = query.GetString("name");
				package.genericId = query.GetInteger("generic_id");
				package.uuid = query.GetString("uuid");
			}			
			query.Release();
			
			return package;
			
		} catch (Exception e)
		{
			UnityEngine.Debug.LogError("Fallo al devolver packages" + e.ToString());
			return package;
		}
		
	}

	/// <summary>
	/// Delete a Package.
	/// </summary>
	/// <returns>The deleted id.</returns>
	/// <param name="package">Package.</param>
	public static int PackageDelete(Package package){
		
		string deleteQueryString = "DELETE FROM packages WHERE guid = "+package.guid.ToString();
		foreach (Quiz quiz in package.quizzes) 
		{
			QuizDelete(quiz);
		}
		try
		{
			SQLiteQuery query = new SQLiteQuery(db,deleteQueryString);
			query.Step();
			query.Release();
			
			return package.guid;
			
		} catch (Exception e)
		{
			UnityEngine.Debug.LogError("Fallo al eliminar package" + e.ToString());
			return -1;
		}
	}

	/// <summary>
	/// Add a new Quiz. Question Container
	/// </summary>
	/// <returns>The Quiz id.</returns>
	/// <param name="quiz">Quiz.</param>
	public static int QuizAdd(Quiz quiz){

		string queryString = "INSERT INTO quizzes (guid, question, correct, answer_type, level_guid, package_guid, modified) VALUES(?,?,?,?,?,?,?)";
		try
		{
			//save data from quizz
			SQLiteQuery query = new SQLiteQuery(db,queryString);
			query.Bind(quiz.guid);
			query.Bind(quiz.question);
			query.Bind(quiz.correct);
			query.Bind(quiz.answerType);
			query.Bind(quiz.levelId);
			query.Bind(quiz.packageGuid);
			query.Bind(quiz.modified);
			query.Step();
			query.Release();
			
			queryString = "SELECT MAX(id) as id FROM quizzes"; //query para seleccionar el ultimo id insertado
			
			query = new SQLiteQuery(db,queryString);
			int quizId = -1;
			while(query.Step())
			{
				quizId = query.GetInteger("id");
			}
			query.Release();

			if(quiz.answers != null)
			{
				foreach(Answer answer in quiz.answers)
				{		
					answer.quizId = quiz.guid;
					if(answer.delete != 1){
						AnswerAdd(answer);
					}
				}
			}
			
			return quizId;
			
		} 
		catch (Exception e)
		{
			UnityEngine.Debug.LogError("Error saving data of Quizz " + e.ToString());
			return -1;
		}
	}

	/// <summary>
	/// get a Quizz by Global id
	/// </summary>
	/// <returns>The Quiz.</returns>
	/// <param name="id">Quiz Global id.</param>
	public static Quiz QuizGet(int guid){
		Quiz quiz = new Quiz();
		quiz.id = -1;
		
		string queryString = "SELECT * FROM quizzes WHERE guid="+guid.ToString();
		try
		{
			SQLiteQuery query = new SQLiteQuery(db,queryString);
			while(query.Step())
			{
				quiz.id = query.GetInteger("id");
				quiz.guid = query.GetInteger("guid");
				quiz.question = query.GetString("question");
				quiz.correct = query.GetInteger("correct");
				quiz.answerType = query.GetString("answer_type");
				quiz.levelId = query.GetInteger("level_guid");
				quiz.packageGuid = query.GetInteger("package_guid");
				quiz.answered = query.GetInteger("answered");
				quiz.points = query.GetInteger("points");
				quiz.modified = query.GetInteger("modified");
				quiz.answers = DatabaseHelper.AnswersGet(quiz.guid);

			}
			query.Release();
			return quiz;
			
		} 
		catch (Exception e)
		{
			UnityEngine.Debug.LogError("Error saving data " + e.ToString());
			return quiz;
		}
	}


	/// <summary>
	/// get a Quizz list by Global id
	/// </summary>
	/// <returns>A Quiz list.</returns>
	/// <param name="packageGuid">Package Global id.</param>
	public static List<Quiz> QuizzesGet(int packageGuid, bool onlyUnanswered = false){
		List<Quiz> quizzes = new List<Quiz>();
		string queryString;
		if (onlyUnanswered)
		{
			queryString = "SELECT * FROM quizzes WHERE answered = 0 AND package_guid="+packageGuid.ToString();
		}else queryString = "SELECT * FROM quizzes WHERE package_guid="+packageGuid.ToString();
		try
		{
			//UnityEngine.Debug.Log ("queryString: "+queryString);
			SQLiteQuery query = new SQLiteQuery(db,queryString);
			while(query.Step())
			{
				Quiz quiz = new Quiz();

				quiz.id = query.GetInteger("id");
				quiz.guid = query.GetInteger("guid");
				quiz.question = query.GetString("question");
				quiz.correct = query.GetInteger("correct");
				quiz.answerType = query.GetString("answer_type");
				quiz.levelId = query.GetInteger("level_guid");
				quiz.packageGuid = query.GetInteger("package_guid");
				quiz.answered = query.GetInteger("answered");
				quiz.points = query.GetInteger("points");
				if(quiz.answerType == "COID" || quiz.answerType == "COIF"){
					quiz.answers = DatabaseHelper.AnswersGet(quiz.guid);
				}

				quizzes.Add(quiz);

			}
			query.Release();
			return quizzes;
			
		} 
		catch (Exception e)
		{
//			UnityEngine.Debug.Log("Error getting quizzes " + e.ToString());
			return quizzes;
		}

	}

	/// <summary>
	/// get a Quizz list by Global id and Level id
	/// </summary>
	/// <returns>A Quiz list.</returns>
	/// <param name="packageGuid">Package Global id.</param>
	public static List<Quiz> QuizzesLevelGet(int packageGuid, int level,  bool onlyUnanswered = false){
		List<Quiz> quizzes = new List<Quiz>();
		string queryString;
		if (onlyUnanswered)
		{
			queryString = "SELECT * FROM quizzes WHERE answered = 0 AND level_guid = "+level+" AND package_guid="+packageGuid.ToString();
		}else queryString = "SELECT * FROM quizzes WHERE  level_guid = "+level+" AND package_guid="+packageGuid.ToString();
		try
		{
			//UnityEngine.Debug.Log ("queryString: "+queryString);
			SQLiteQuery query = new SQLiteQuery(db,queryString);
			while(query.Step())
			{
				Quiz quiz = new Quiz();
				
				quiz.id = query.GetInteger("id");
				quiz.guid = query.GetInteger("guid");
				quiz.question = query.GetString("question");
				quiz.correct = query.GetInteger("correct");
				quiz.answerType = query.GetString("answer_type");
				quiz.levelId = query.GetInteger("level_guid");
				quiz.packageGuid = query.GetInteger("package_guid");
				quiz.answered = query.GetInteger("answered");
				quiz.points = query.GetInteger("points");
				if(quiz.answerType == "COID" || quiz.answerType == "COIF"){
					quiz.answers = DatabaseHelper.AnswersGet(quiz.guid);
				}
				
				quizzes.Add(quiz);
				
			}
			query.Release();
			return quizzes;
			
		} 
		catch (Exception e)
		{
			//			UnityEngine.Debug.Log("Error getting quizzes " + e.ToString());
			return quizzes;
		}
		
	}

	/// <summary>
	/// update a Quiz.
	/// </summary>
	/// <returns>The updated id.</returns>
	/// <param name="quiz">Quiz.</param>
	public static int QuizUpdate(Quiz quiz){

		string updateQueryString = "UPDATE quizzes set level_guid="+quiz.levelId+", correct="+quiz.correct+", answer_type='"+quiz.answerType+"', question='"+quiz.question+"', modified="+quiz.modified+" WHERE guid = "+quiz.guid.ToString();

		string answerIds = "";
		int totalAnswers = quiz.answers.Count;
		int count = 0;
		foreach (Answer answer in quiz.answers) 
		{
			if(count == totalAnswers-1){
				answerIds+= answer.guid.ToString();
			}else {
				answerIds+= answer.guid.ToString()+", ";
			}
			Answer answerSaved = AnswerGetByGuid(answer.guid);
			//UnityEngine.Debug.Log("La answerSaved: "+answerSaved.id);
			if(answerSaved.id != -1){
				//UnityEngine.Debug.Log("La respuesta: "+answer.modified+" saved:"+answerSaved.modified);
				if(answer.modified>answerSaved.modified){
					//UnityEngine.Debug.Log("La respuesta ha sido modificada: "+answer.answer);
					AnswerUpdate(answer);
				}
				//answerSaved = null;
			}else{
				//UnityEngine.Debug.Log("La respuesta ha sido agregada: "+answer.answer);
				AnswerAdd(answer);
			}
			count++;
		}
		//UnityEngine.Debug.Log("Eliminar Respuestas: "+answerIds);
		AnswerDeleteByIds (quiz.guid, answerIds);

		try
		{
			SQLiteQuery query = new SQLiteQuery(db,updateQueryString);
			query.Step();
			query.Release();
			
			return quiz.guid;
			
		} catch (Exception e)
		{
			UnityEngine.Debug.Log("Fallo al editar quiz" + e.ToString());
			return -1;
		}
	}

	/// <summary>
	/// Quiz the delete.
	/// </summary>
	/// <returns>The delete.</returns>
	/// <param name="quiz">Quiz.</param>
	public static int QuizDelete(Quiz quiz){

		string deleteQueryString = "DELETE FROM quizzes WHERE guid = "+quiz.guid.ToString();
		if (quiz.answers != null) {
			foreach (Answer answer in quiz.answers) 
			{
				AnswerDelete(answer);
			}
		}

		try
		{
			SQLiteQuery query = new SQLiteQuery(db,deleteQueryString);
			query.Step();
			query.Release();
			
			return quiz.guid;
			
		} catch (Exception e)
		{
			UnityEngine.Debug.Log("Fallo al eliminar quiz" + e.ToString());
			return -1;
		}
	}

	/// <summary>
	/// set points to a quiz
	/// </summary>
	/// <returns>the id of quiz modified.</returns>
	/// <param name="quiz">Quiz.</param>
	public static int QuizSetPoints(Quiz quiz){

		string queryString = "UPDATE quizzes SET points = "+quiz.points+" WHERE guid="+quiz.guid.ToString();

		try
		{
			SQLiteQuery query = new SQLiteQuery(db,queryString);
			query.Step();
			query.Release();			
			return quiz.guid;
			
		} catch (Exception e)
		{
			UnityEngine.Debug.Log("Fallo al guardar " + e.ToString());
			return -1;
		}
	}

	/// <summary>
	/// set correct answer to a quiz
	/// </summary>
	/// <returns>the id of quiz modified.</returns>
	/// <param name="quiz">Quiz.</param>
	public static int QuizSetAnswered(Quiz quiz){
		
		string queryString = "UPDATE quizzes SET answered = "+quiz.answered+" WHERE guid="+quiz.guid.ToString();
		
		try
		{
			SQLiteQuery query = new SQLiteQuery(db,queryString);
			query.Step();
			query.Release();			
			return quiz.guid;
			
		} catch (Exception e)
		{
			UnityEngine.Debug.Log("Fallo al guardar " + e.ToString());
			return -1;
		}
	}

	/// <summary>
	/// Add a new Answer.
	/// </summary>
	/// <returns>The Answer id.</returns>
	/// <param name="answer">Answer.</param>
	public static int AnswerAdd(Answer answer){

		string queryString = "INSERT INTO answers (guid, name, quiz_id, modified) VALUES(?,?,?,?)";
		try
		{
			SQLiteQuery query = new SQLiteQuery(db,queryString);
			query.Bind(answer.guid);
			query.Bind(answer.answer);
			query.Bind(answer.quizId);
			query.Bind(answer.modified);
			query.Step();
			query.Release();

			queryString = "SELECT MAX(id) as id FROM answers"; //query para seleccionar el ultimo id insertado

			query = new SQLiteQuery(db,queryString); //inserto el query
			int id = -1;
			while(query.Step())
			{
				id = query.GetInteger("id");
			}
			query.Release();

			return id;
			
		} 
		catch (Exception e)
		{
			UnityEngine.Debug.Log("Error saving data " + e.ToString());
			return -1;
		}
	}

	/// <summary>
	/// get an Answers List of a Quizz.
	/// </summary>
	/// <returns>A Answers List.</returns>
	/// <param name="quizGuid">Quiz Global identifier.</param>
	public static List<Answer> AnswersGet(int quizGuid){
		List<Answer> answers = new List<Answer>();

		string queryString = "SELECT * FROM answers WHERE quiz_id="+quizGuid.ToString();
		try
		{
			SQLiteQuery query = new SQLiteQuery(db,queryString);
			while(query.Step())
			{
				Answer answer = new Answer();
				answer.id = query.GetInteger("id");
				answer.guid = query.GetInteger("guid");
				answer.quizId = query.GetInteger("quiz_id");
				answer.answer = query.GetString("name");
				answer.modified = query.GetInteger("modified");
				answers.Add(answer);
			}
			query.Release();			
			return answers;
			
		} 
		catch (Exception e)
		{
			UnityEngine.Debug.Log("Error saving data " + e.ToString());
			return answers;
		}
	}

	/// <summary>
	/// get an Answer of a Quizz by Global id.
	/// </summary>
	/// <returns>The Answer.</returns>
	/// <param name="guid">Global id.</param>
	public static Answer AnswerGetByGuid(int guid){
		Answer answer = new Answer();
		answer.id = -1;
		
		string queryString = "SELECT * FROM answers WHERE guid="+guid.ToString();
		try
		{
			SQLiteQuery query = new SQLiteQuery(db,queryString);
			while(query.Step())
			{
				answer.id = query.GetInteger("id");
				answer.guid = query.GetInteger("guid");
				answer.quizId = query.GetInteger("quiz_id");
				answer.answer = query.GetString("name");
				answer.modified = query.GetInteger("modified");

			}
			query.Release();			
			return answer;
			
		} 
		catch (Exception e)
		{
			UnityEngine.Debug.Log("Error saving data " + e.ToString());
			return answer;
		}
	}

	/// <summary>
	/// update the Answer of a Quizz.
	/// </summary>
	/// <returns>The id.</returns>
	/// <param name="Answer">answer.</param>
	public static int AnswerUpdate(Answer answer){

		string updateQueryString = "UPDATE answers SET name = '"+answer.answer+"' WHERE guid = "+answer.guid.ToString();
		try
		{
			SQLiteQuery query = new SQLiteQuery(db,updateQueryString);
			query.Step();
			query.Release();

			return answer.guid;
			
		} catch (Exception e)
		{
			UnityEngine.Debug.Log("Fallo al guardar " + e.ToString());
			return -1;
		}
	}

	/// <summary>
	/// Answers the delete.
	/// </summary>
	/// <returns>The delete.</returns>
	/// <param name="answer">Answer.</param>
	public static int AnswerDelete(Answer answer){
		
		string deleteQueryString = "DELETE FROM answers WHERE guid = "+answer.guid.ToString();
		try
		{
			SQLiteQuery query = new SQLiteQuery(db,deleteQueryString);
			query.Step();
			query.Release();
			
			return answer.guid;
			
		} catch (Exception e)
		{
			UnityEngine.Debug.Log("Fallo al eliminar " + e.ToString());
			return -1;
		}
	}


	/// Answers the delete.
	/// </summary>
	/// <returns>The delete.</returns>
	/// <param name="answer">Answer.</param>
	public static int AnswerDeleteByIds(int quizId, string ids){
		try
		{
			//UnityEngine.Debug.Log ("DELETE ANSWERS ID:"+ids);
			string deleteString = "delete from answers where answers.quiz_id = "+quizId.ToString()+" AND answers.guid NOT IN ("+ids+");";
			SQLiteQuery query = new SQLiteQuery(db,deleteString);
			query.Step();
			query.Release();
			return 1;
			
		} catch (Exception e)
		{
			UnityEngine.Debug.Log("Fallo al editar quiz" + e.ToString());
			return -1;
		}
	}

	/// <summary>
	/// get the Levels score.
	/// </summary>
	/// <returns>the score.</returns>
	/// <param name="levelId">Level identifier.</param>
	/// <param name="packageId">Package identifier.</param>
	public static int LevelGetScore(int levelId, int packageId){
		
		int score = 0;
		string queryString = "SELECT SUM(points) score FROM quizzes WHERE level_guid = "+levelId.ToString()+" AND package_guid = "+packageId.ToString();
		try
		{
			SQLiteQuery query = new SQLiteQuery(db,queryString);
			while(query.Step())
			{
				score = query.GetInteger("score");
			}
			query.Release();
			return score;
			
		} 
		catch (Exception e)
		{
			UnityEngine.Debug.Log("Error getting score " + e.ToString());
			return score;
		}
	}

	/// <summary>
	/// return the list of levels with de completed or not.
	/// </summary>
	/// <returns>get if level is finished or not.</returns>
	/// <param name="packageId">Package identifier.</param>
	public static List<Level> LevelsGetFinished(int packageId){

		List<Level> levels = new List<Level> ();
		for (int i = 0; i<10; i++) {
			int levelNumber = i+1;
			Level level = LevelGetFinished(levelNumber, packageId);

			levels.Add(level);
		}
		return levels;
	}

	/// <summary>
	/// return if level is completed.
	/// </summary>
	/// <returns>1 if level complete or 0 if level not complete.</returns>
	/// <param name="levelId">Level identifier.</param>
	/// <param name="packageId">Package identifier.</param>
	public static Level LevelGetFinished(int levelId, int packageId){


		int points = 0;
		int totalQuestions = 0;
		Level level = new Level ();
		level.level = levelId;
		level.packageId = packageId;
		level.score = 0;
		level.complete = 0;
		string queryString = "SELECT COUNT(*) as total FROM quizzes WHERE level_guid = "+levelId.ToString()+" AND package_guid = "+packageId.ToString()+" AND answered = 1";
		try
		{
			SQLiteQuery query = new SQLiteQuery(db,queryString);
			query.Step();
			totalQuestions = query.GetInteger("total");
			query.Release();
			if(totalQuestions < 20){
				return level;
			}
		} 
		catch (Exception e)
		{
			UnityEngine.Debug.Log("Error getting total answered " + e.ToString());
			return level;
		}

		try{
			queryString = "SELECT SUM(q.points) AS points FROM quizzes q WHERE q.package_guid = 2229 AND q.level_guid = 1";
			query = new SQLiteQuery(db,queryString);
			query.Step();
			points = query.GetInteger("points");
			query.Release();
		} 
		catch (Exception e)
		{
			UnityEngine.Debug.Log("Error getting total answered " + e.ToString());
			return level;
		}

		level.complete = 1;
		level.score = points;

		return level;

	}


	/// <summary>
	/// Gets the last modified.
	/// </summary>
	/// <returns>The last modified.</returns>
	public static int GetLastModified(string uuid){
		
		int modified = 0;
		string queryString = "SELECT modified FROM packages WHERE uuid='"+uuid+"' ORDER BY modified ASC LIMIT 1";
		try
		{
			SQLiteQuery query = new SQLiteQuery(db,queryString);
			while(query.Step())
			{
				modified = query.GetInteger("modified");
			}
			query.Release();
			return modified;			
		} 
		catch (Exception e)
		{
			UnityEngine.Debug.Log("Error getting modified date " + e.ToString());
			return modified;
		}
	}

	/// <summary>
	/// Gets packages downloaded.
	/// </summary>
	/// <returns>The last modified.</returns>
	public static int GetTotalDownloadedPackages(){
		
		int total = 0;

		string queryString = "SELECT count(*) total FROM products WHERE downloaded = 1";
		try
		{
			SQLiteQuery query = new SQLiteQuery(db,queryString);
			while(query.Step())
			{
				total = query.GetInteger("total");
			}
			query.Release();
			return total;			
		} 
		catch (Exception e)
		{
			UnityEngine.Debug.Log("Error getting modified date " + e.ToString());
			return total;
		}
	}



	public static int GetOwnedProduct(string id){
		int owned = 0;
		string queryString = "SELECT * FROM products WHERE name = '"+id+"'";
		try
		{
			SQLiteQuery query = new SQLiteQuery(db,queryString);
			while(query.Step())
			{
				owned = query.GetInteger("owned");
			}
			query.Release();
			return owned;			
		} 
		catch (Exception e)
		{
			UnityEngine.Debug.Log("Error getting owned product " + e.ToString());
			return owned;
		}
	}

	public static List<Product> GetOwnedForDownload(){
		List<Product> products = new List<Product>();
		string queryString = "SELECT * FROM products WHERE owned = 1 AND downloaded = 0 AND premium = 1";
		try
		{
			SQLiteQuery query = new SQLiteQuery(db,queryString);
			while(query.Step())
			{
				Product product = new Product();
				product.id = query.GetInteger("id");
				product.guid = query.GetInteger("guid");
				product.category  = query.GetInteger("tid");
				product.downloaded = query.GetInteger("downloaded");
				product.owned = query.GetInteger("owned");
				product.name = query.GetString("name");
				product.premium = query.GetInteger("premium");
				product.modified = query.GetInteger("modified");
				product.uuid = query.GetString("uuid");
				products.Add(product);
			}
			query.Release();
			return products;			
		} 
		catch (Exception e)
		{
			UnityEngine.Debug.Log("Error getting owned products " + e.ToString());
			return products;
		}
	}

	/// <summary>
	/// get the num of UpTime Power Up.
	/// </summary>
	/// <returns>the number of update time Power Up .</returns>
	public static int PowerUpTimeGet(){
		string queryString = "SELECT * FROM power_ups WHERE id='TIM'";
		int value = 0;
		try
		{
			SQLiteQuery query = new SQLiteQuery(db,queryString);
			while(query.Step())
			{
				value = query.GetInteger("value");
			}
			query.Step();
			query.Release();
			return value;
		} 
		catch (Exception e)
		{
			UnityEngine.Debug.Log("Error getting update time powerUp" + e.ToString());
			return value;
		}
	}

	/// <summary>
	/// update time PowerUp.
	/// </summary>
	/// <returns>The powerUp time update.</returns>
	/// <param name="powerUp">Power up.</param>
	public static int PowerUpTimeSet(int value){
		string queryString = "update power_ups set value = "+value+" where id = 'TIM'";
		try
		{
			SQLiteQuery query = new SQLiteQuery(db,queryString);
			query.Step();
			query.Release();
			return 1;			
		} 
		catch (Exception e)
		{
			UnityEngine.Debug.Log("Error getting time" + e.ToString());
			return -1;
		}
	}

	/// <summary>
	/// get the multiply score Power Up.
	/// </summary>
	/// <returns>the number of multiply score Power Up .</returns>
	public static int PowerUpMultyplyScoreGet(){
		string queryString = "SELECT * FROM power_ups WHERE id='DUP'";
		int value = 0;
		try
		{
			SQLiteQuery query = new SQLiteQuery(db,queryString);
			while(query.Step())
			{
				value = query.GetInteger("value");
			}
			query.Release();
			return value;
		} 
		catch (Exception e)
		{
			UnityEngine.Debug.Log("Error getting multipy powerUp" + e.ToString());
			return value;
		}
	}

	/// <summary>
	/// update multiplyScore PowerUp.
	/// </summary>
	/// <returns>void .</returns>
	/// <param name="powerUp">Integer.</param>
	public static int PowerUpMultiplyScoreSet(int value){
		string queryString = "update power_ups set value = "+value+" where id = 'DUP'";
		try
		{
			SQLiteQuery query = new SQLiteQuery(db,queryString);
			query.Step();
			query.Release();
			return 1;			
		} 
		catch (Exception e)
		{
			UnityEngine.Debug.Log("Error updating multiply score" + e.ToString());
			return -1;
		}
	}

	/// <summary>
	/// get the num of Victory Power Up.
	/// </summary>
	/// <returns>the number of multiply score Power Up .</returns>
	public static int PowerUpVictoryGet(){
		string queryString = "SELECT * FROM power_ups WHERE id='REM'";
		int value = 0;
		try
		{
			SQLiteQuery query = new SQLiteQuery(db,queryString);
			while(query.Step())
			{
				value = query.GetInteger("value");
			}
			query.Step();
			query.Release();
			return value;
		} 
		catch (Exception e)
		{
			UnityEngine.Debug.Log("Error getting victory powerUp" + e.ToString());
			return value;
		}
	}


	/// <summary>
	/// update victory PowerUp.
	/// </summary>
	/// <returns>The up time update.</returns>
	/// <param name="powerUp">Power up.</param>
	public static int PowerUpVictorySet(int value){
		string queryString = "update power_ups set value = "+value+" where id = 'REM'";
		try
		{
			SQLiteQuery query = new SQLiteQuery(db,queryString);
			query.Step();
			query.Release();
			return 1;			
		} 
		catch (Exception e)
		{
			UnityEngine.Debug.Log("Error updating victory" + e.ToString());
			return -1;
		}
	}

	/// <summary>
	/// return a List of Feats.
	/// </summary>
	/// <returns>The get List of Feats.</returns>
	public static List<Feat> FeatsGet(){
		string queryString = "SELECT * FROM feats";
		List<Feat> feats = new List<Feat>();

		try
		{
			SQLiteQuery query = new SQLiteQuery(db,queryString);
			while(query.Step())
			{
				Feat feat = new Feat();
				feat.id = query.GetString("id");
				feat.key = query.GetString("key");
				feat.image_off = query.GetString("image_off");
				feat.image_on = query.GetString("image_on");
				feat.completed = query.GetInteger("completed");
				if(!query.IsNULL("guid")){
					feat.guid = query.GetInteger("guid");
				}


				feats.Add(feat);
			}
			query.Step();
			query.Release();
			return feats;
		} 
		catch (Exception e)
		{
			UnityEngine.Debug.Log("Error getting victory powerUp" + e.ToString());
			return feats;
		}
	}

	/// <summary>
	/// Set a feats completed.
	/// </summary>
	/// <returns>The set.</returns>
	/// <param name="id">Identifier.</param>
	public static int FeatSet(string id){
		string queryString = "update feats set completed = 1 where id = '"+id+"'";
		try
		{
			SQLiteQuery query = new SQLiteQuery(db,queryString);
			query.Step();
			query.Release();
			return 1;
		} 
		catch (Exception e)
		{
			UnityEngine.Debug.Log("Error asigning feat" + e.ToString());
			return -1;
		}
	}

	/// <summary>
	/// get the configs.
	/// </summary>
	/// <returns>The get.</returns>
	/// <param name="key">Key.</param>
	public static int ConfigSet(string key, string value, bool update = false){
		try
		{
			SQLiteQuery qr;
			if(!update){
				//UnityEngine.Debug.Log ("NEW LANG");
				qr = new SQLiteQuery(db,"INSERT INTO configs (key_name, key_value) VALUES(?,?);");
				qr.Bind(key);
				qr.Bind(value);
			}else {
				//UnityEngine.Debug.Log ("UPDATE LANG");
				qr = new SQLiteQuery(db,"UPDATE configs SET key_name='"+key+"', key_value='"+value+"'");
			}

			qr.Step();
			qr.Release();
			
			return 1;
		} 
		catch (Exception e)
		{
			UnityEngine.Debug.Log("Error setting config" + e.ToString());
			return -1;
		}
	}

	/// <summary>
	/// get the configs.
	/// </summary>
	/// <returns>The get.</returns>
	/// <param name="key">Key.</param>
	public static Hashtable ConfigGet(string key){
		Hashtable config = new Hashtable ();
		config ["status"] = -1;

		string queryString = "SELECT COUNT(*) number FROM configs where key_name = '"+key+"'";
		//string queryString = "SELECT * FROM configs";
		try
		{
			SQLiteQuery query = new SQLiteQuery(db,queryString);
			query.Step();
			int number = query.GetInteger("number");
			//UnityEngine.Debug.Log ("number:"+number);
			if(number == 0){
				config ["status"] = -1;
			}else{
				queryString = "SELECT * FROM configs where key_name = '"+key+"'";


				SQLiteQuery query2 = new SQLiteQuery(db,queryString);
				query2.Step();
				if(!query2.IsNULL("key_value")){
					//UnityEngine.Debug.Log("no es nulo ");
					config ["value"] = query2.GetString("key_value");
					config ["status"] = 1;
				}else{
					config ["status"] = -1;
					//UnityEngine.Debug.Log("es nulo ");
				}
				
				query2.Release();
			}
			
			return config;
		} 
		catch (Exception e)
		{
			UnityEngine.Debug.Log("Error getting config" + e.ToString());
			config ["status"] = -1;
			return config;
		}
		
	}*/

}