-- This file contain the create index statements for the Nippon File Transfer project @ Dualog.

CREATE INDEX "ftdui_transferjob_default_source_id" ON "ftdui_transferjob" ("default_source_id");
CREATE INDEX "ftdui_source_client_id" ON "ftdui_source" ("client_id");
CREATE INDEX "ftdui_source_folder_id" ON "ftdui_source" ("folder_id");
CREATE INDEX "ftdui_source_job_id" ON "ftdui_source" ("job_id");
CREATE INDEX "ftdui_destination_client_id" ON "ftdui_destination" ("client_id");
CREATE INDEX "ftdui_destination_folder_id" ON "ftdui_destination" ("folder_id");
CREATE INDEX "ftdui_destination_job_id" ON "ftdui_destination" ("job_id");
CREATE INDEX "ftdui_client_key_id" ON "ftdui_client" ("key_id");






