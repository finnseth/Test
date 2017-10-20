-- This file contain the create statements for the Nippon File Transfer project @ Dualog.


CREATE TABLE 
"ftdui_folder" (
	"id" integer IDENTITY(1,1) NOT NULL PRIMARY KEY, 
	"mount_source" text NOT NULL, 
	"mount_type" varchar(256) NOT NULL, 
	"mount_options" text NOT NULL
);


CREATE TABLE 
"ftdui_key" 
(
	"id" integer IDENTITY(1,1) NOT NULL PRIMARY KEY,
	 "hash" varchar(256) NOT NULL, 
	 "content" text NOT NULL
);

CREATE TABLE "ftdui_transferjob" 
(
	"id" integer IDENTITY(1,1) NOT NULL PRIMARY KEY, 
	"name" varchar(256) NOT NULL, 
	"default_destination_id" integer NULL REFERENCES "ftdui_folder" ("id"), 
	"default_source_id" integer NULL REFERENCES "ftdui_folder" ("id")
);


CREATE TABLE "ftdui_client"
(
	"id" integer IDENTITY(1,1) NOT NULL PRIMARY KEY, 
	"client_id" varchar(256) NOT NULL, 
	"key_id" integer NOT NULL REFERENCES "ftdui_key" ("id")
);



CREATE TABLE "ftdui_source" 
(
	"id" integer IDENTITY(1,1) NOT NULL PRIMARY KEY, 
	"client_id" integer NOT NULL REFERENCES "ftdui_client" ("id"), 
	"folder_id" integer NULL REFERENCES "ftdui_folder" ("id"), 
	"job_id" integer NOT NULL REFERENCES "ftdui_transferjob" ("id")
);

CREATE TABLE "ftdui_destination"
(
	"id" integer IDENTITY(1,1) NOT NULL PRIMARY KEY, 
	"client_id" integer NOT NULL REFERENCES "ftdui_client" ("id"), 
	"folder_id" integer NULL REFERENCES "ftdui_folder" ("id"), 
	"job_id" integer NOT NULL REFERENCES "ftdui_transferjob" ("id")
);

