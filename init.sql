-- init.sql

-- Create a Users table
CREATE TABLE IF NOT EXISTS "Users" (
    "Id" uuid NOT NULL,
    "Name" text NOT NULL,
    "Surname" text NOT NULL,
    "Password" text NOT NULL,
    "Email" text NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
);

INSERT INTO "Users" ("Id", "Name", "Surname", "Password", "Email", "CreatedAt", "UpdatedAt") VALUES
('3d162347-61ac-475a-a56a-7fa0bb98daba', 'Stefan', 'Turner', 'QTKL8Opd', 'stefan_turner38@yahoo.com', '2024-05-28 13:35:21.755029+00', '-infinity'),
('cebfbfb6-8b16-410c-abda-caf520789547', 'Devonte', 'Jerde', 'dAfvJkBT', 'devonte.jerde@yahoo.com', '2024-05-28 13:35:21.926345+00', '-infinity'),
('9f102334-48fa-4a9d-8d14-fffd779aae5f', 'Reanna', 'Stoltenberg', 'g6FdbDov', 'reanna.stoltenberg46@hotmail.com', '2024-05-28 13:35:21.93081+00', '-infinity'),
('9cf6a0f9-c63a-4d49-b3ca-b9ac99ab4597', 'Maye', 'Mayert', 'ZRtWoKv6', 'maye_mayert77@gmail.com', '2024-05-28 13:35:21.958236+00', '-infinity'),
('6af8f659-3e38-40a8-b10a-b438d91f8c7e', 'Nicklaus', 'Stokes', 'fB6SKaR_', 'nicklaus_stokes37@hotmail.com', '2024-05-28 13:35:22.010371+00', '-infinity'),
('77257d24-46e6-4a3e-846c-3451a2e7a6c5', 'Percival', 'Wisoky', 'zGhl7j38', 'percival_wisoky63@hotmail.com', '2024-05-28 13:35:22.038419+00', '-infinity'),
('5756dc44-3c7a-4a57-9d56-3cf63875ebc6', 'Bertha', 'Reichel', 'Wa2uZzK2', 'bertha.reichel79@gmail.com', '2024-05-28 13:35:22.041143+00', '-infinity');
