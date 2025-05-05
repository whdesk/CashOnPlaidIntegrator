-- Crear tabla de Categorías de Plaid
CREATE TABLE PlaidCategories (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PrimaryCategory NVARCHAR(50) NOT NULL,
    DetailedCategory NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
);

-- Insertar categorías de INCOME
INSERT INTO PlaidCategories (PrimaryCategory, DetailedCategory, Description)
VALUES 
    ('INCOME', 'INCOME_WAGES', 'Ingresos de salarios, trabajos de economía colaborativa y propinas ganadas'),
    ('INCOME', 'INCOME_RETIREMENT_PENSION', 'Ingresos de pagos de pensiones'),
    ('INCOME', 'INCOME_UNEMPLOYMENT', 'Ingresos de beneficios por desempleo, incluyendo seguro de desempleo y atención médica'),
    ('INCOME', 'INCOME_TAX_REFUND', 'Ingresos de reembolsos de impuestos'),
    ('INCOME', 'INCOME_DIVIDENDS', 'Dividendos de cuentas de inversión'),
    ('INCOME', 'INCOME_INTEREST_EARNED', 'Ingresos de intereses en cuentas de ahorros'),
    ('INCOME', 'INCOME_OTHER_INCOME', 'Otros ingresos diversos, incluyendo alimentos, seguridad social, manutención y alquiler');

-- Insertar categorías de TRANSFER_IN
INSERT INTO PlaidCategories (PrimaryCategory, DetailedCategory, Description)
VALUES 
    ('TRANSFER_IN', 'TRANSFER_IN_ACCOUNT_TRANSFER', 'Transferencias generales desde otra cuenta'),
    ('TRANSFER_IN', 'TRANSFER_IN_CASH_ADVANCES_AND_LOANS', 'Préstamos y avances en efectivo depositados en una cuenta bancaria'),
    ('TRANSFER_IN', 'TRANSFER_IN_DEPOSIT', 'Depósitos de efectivo, cheques y cajeros automáticos en una cuenta bancaria'),
    ('TRANSFER_IN', 'TRANSFER_IN_INVESTMENT_AND_RETIREMENT_FUNDS', 'Transferencias entrantes a una cuenta de inversión o retiro'),
    ('TRANSFER_IN', 'TRANSFER_IN_SAVINGS', 'Transferencias entrantes a una cuenta de ahorros'),
    ('TRANSFER_IN', 'TRANSFER_IN_OTHER_TRANSFER_IN', 'Otras transacciones entrantes diversas');

-- Insertar categorías de TRANSFER_OUT
INSERT INTO PlaidCategories (PrimaryCategory, DetailedCategory, Description)
VALUES 
    ('TRANSFER_OUT', 'TRANSFER_OUT_INVESTMENT_AND_RETIREMENT_FUNDS', 'Transferencias salientes a una cuenta de inversión o retiro, incluyendo aplicaciones de inversión como Acorns, Betterment'),
    ('TRANSFER_OUT', 'TRANSFER_OUT_SAVINGS', 'Transferencias salientes a cuentas de ahorros'),
    ('TRANSFER_OUT', 'TRANSFER_OUT_WITHDRAWAL', 'Retiros de una cuenta bancaria'),
    ('TRANSFER_OUT', 'TRANSFER_OUT_ACCOUNT_TRANSFER', 'Transferencias salientes generales a otra cuenta'),
    ('TRANSFER_OUT', 'TRANSFER_OUT_OTHER_TRANSFER_OUT', 'Otras transacciones salientes diversas');

-- Insertar categorías de LOAN_PAYMENTS
INSERT INTO PlaidCategories (PrimaryCategory, DetailedCategory, Description)
VALUES 
    ('LOAN_PAYMENTS', 'LOAN_PAYMENTS_AUTO', 'Pagos de préstamos de automóviles'),
    ('LOAN_PAYMENTS', 'LOAN_PAYMENTS_CREDIT_CARD', 'Pagos de tarjetas de crédito'),
    ('LOAN_PAYMENTS', 'LOAN_PAYMENTS_MORTGAGE', 'Pagos de hipotecas'),
    ('LOAN_PAYMENTS', 'LOAN_PAYMENTS_PERSONAL', 'Pagos de préstamos personales'),
    ('LOAN_PAYMENTS', 'LOAN_PAYMENTS_STUDENT', 'Pagos de préstamos estudiantiles'),
    ('LOAN_PAYMENTS', 'LOAN_PAYMENTS_OTHER_LOAN_PAYMENTS', 'Otros pagos de préstamos diversos');

-- Insertar categorías de BANK_FEES
INSERT INTO PlaidCategories (PrimaryCategory, DetailedCategory, Description)
VALUES 
    ('BANK_FEES', 'BANK_FEES_ATM', 'Tarifas de cajeros automáticos'),
    ('BANK_FEES', 'BANK_FEES_FOREIGN_TRANSACTION', 'Tarifas de transacciones internacionales'),
    ('BANK_FEES', 'BANK_FEES_OVERDRAFT', 'Tarifas de sobregiro'),
    ('BANK_FEES', 'BANK_FEES_SERVICE', 'Tarifas de servicio'),
    ('BANK_FEES', 'BANK_FEES_OTHER_BANK_FEES', 'Otras tarifas bancarias diversas');

-- Insertar categorías de ENTERTAINMENT
INSERT INTO PlaidCategories (PrimaryCategory, DetailedCategory, Description)
VALUES 
    ('ENTERTAINMENT', 'ENTERTAINMENT_GAMBLING', 'Casinos, loterías y apuestas'),
    ('ENTERTAINMENT', 'ENTERTAINMENT_MUSIC', 'Conciertos, discotecas y tiendas de música'),
    ('ENTERTAINMENT', 'ENTERTAINMENT_SPORTING_EVENTS', 'Eventos deportivos, incluyendo boletos y equipos'),
    ('ENTERTAINMENT', 'ENTERTAINMENT_THEATER_AND_MOVIES', 'Teatro, cine y streaming'),
    ('ENTERTAINMENT', 'ENTERTAINMENT_OTHER_ENTERTAINMENT', 'Otro entretenimiento diverso, incluyendo videojuegos y clubes');

-- Insertar categorías de FOOD_AND_DRINK
INSERT INTO PlaidCategories (PrimaryCategory, DetailedCategory, Description)
VALUES 
    ('FOOD_AND_DRINK', 'FOOD_AND_DRINK_FOOD_DELIVERY', 'Pedidos de comida a domicilio'),
    ('FOOD_AND_DRINK', 'FOOD_AND_DRINK_GROCERIES', 'Supermercados y tiendas de comestibles'),
    ('FOOD_AND_DRINK', 'FOOD_AND_DRINK_RESTAURANTS', 'Restaurantes y bares'),
    ('FOOD_AND_DRINK', 'FOOD_AND_DRINK_VENDING_MACHINES', 'Maquinas expendedoras'),
    ('FOOD_AND_DRINK', 'FOOD_AND_DRINK_OTHER_FOOD_AND_DRINK', 'Otro comida y bebida diverso, incluyendo catering y comida rápida');

-- Insertar categorías de GENERAL_MERCHANDISE
INSERT INTO PlaidCategories (PrimaryCategory, DetailedCategory, Description)
VALUES 
    ('GENERAL_MERCHANDISE', 'GENERAL_MERCHANDISE_BOOKS', 'Librerías y tiendas de libros'),
    ('GENERAL_MERCHANDISE', 'GENERAL_MERCHANDISE_CLOTHING', 'Tiendas de ropa y accesorios'),
    ('GENERAL_MERCHANDISE', 'GENERAL_MERCHANDISE_CONVENIENCE_STORES', 'Tiendas de conveniencia'),
    ('GENERAL_MERCHANDISE', 'GENERAL_MERCHANDISE_DEPARTMENT_STORES', 'Tiendas departamentales'),
    ('GENERAL_MERCHANDISE', 'GENERAL_MERCHANDISE_DISCOUNT_STORES', 'Tiendas de descuento'),
    ('GENERAL_MERCHANDISE', 'GENERAL_MERCHANDISE_ELECTRONICS', 'Tiendas de electrónica'),
    ('GENERAL_MERCHANDISE', 'GENERAL_MERCHANDISE_MALLS', 'Centros comerciales'),
    ('GENERAL_MERCHANDISE', 'GENERAL_MERCHANDISE_OTHER_GENERAL_MERCHANDISE', 'Otro comercio general diverso, incluyendo tiendas de regalos y juguetes');

-- Insertar categorías de HOME_IMPROVEMENT
INSERT INTO PlaidCategories (PrimaryCategory, DetailedCategory, Description)
VALUES 
    ('HOME_IMPROVEMENT', 'HOME_IMPROVEMENT_FURNITURE', 'Muebles, ropa de cama y accesorios para el hogar'),
    ('HOME_IMPROVEMENT', 'HOME_IMPROVEMENT_HARDWARE', 'Materiales de construcción, tiendas de hardware, pintura y papel tapiz'),
    ('HOME_IMPROVEMENT', 'HOME_IMPROVEMENT_REPAIR_AND_MAINTENANCE', 'Fontanería, iluminación, jardinería y techado'),
    ('HOME_IMPROVEMENT', 'HOME_IMPROVEMENT_SECURITY', 'Compras de sistemas de seguridad para el hogar'),
    ('HOME_IMPROVEMENT', 'HOME_IMPROVEMENT_OTHER_HOME_IMPROVEMENT', 'Otras compras de mejoras para el hogar, incluyendo instalación de piscinas y control de plagas');

-- Insertar categorías de MEDICAL
INSERT INTO PlaidCategories (PrimaryCategory, DetailedCategory, Description)
VALUES 
    ('MEDICAL', 'MEDICAL_DENTAL_CARE', 'Dentistas y cuidado dental general'),
    ('MEDICAL', 'MEDICAL_EYE_CARE', 'Optometristas, lentes de contacto y tiendas de gafas'),
    ('MEDICAL', 'MEDICAL_NURSING_CARE', 'Cuidado y facilidades de enfermería'),
    ('MEDICAL', 'MEDICAL_PHARMACIES_AND_SUPPLEMENTS', 'Farmacias y tiendas de nutrición'),
    ('MEDICAL', 'MEDICAL_PRIMARY_CARE', 'Médicos y médicos'),
    ('MEDICAL', 'MEDICAL_VETERINARY_SERVICES', 'Procedimientos de prevención y cuidado para animales'),
    ('MEDICAL', 'MEDICAL_OTHER_MEDICAL', 'Otro médico diverso, incluyendo trabajo de sangre, hospitales y ambulancias');

-- Insertar categorías de PERSONAL_CARE
INSERT INTO PlaidCategories (PrimaryCategory, DetailedCategory, Description)
VALUES 
    ('PERSONAL_CARE', 'PERSONAL_CARE_GYMS_AND_FITNESS_CENTERS', 'Gimnasios, centros de fitness y clases de entrenamiento'),
    ('PERSONAL_CARE', 'PERSONAL_CARE_HAIR_AND_BEAUTY', 'Manicuras, cortes de pelo, depilación, spa/masajes y productos de baño y belleza'),
    ('PERSONAL_CARE', 'PERSONAL_CARE_LAUNDRY_AND_DRY_CLEANING', 'Gastos de lavado y planchado, y limpieza en seco'),
    ('PERSONAL_CARE', 'PERSONAL_CARE_OTHER_PERSONAL_CARE', 'Otro cuidado personal diverso, incluyendo aplicaciones y servicios de salud mental');

-- Insertar categorías de GENERAL_SERVICES
INSERT INTO PlaidCategories (PrimaryCategory, DetailedCategory, Description)
VALUES 
    ('GENERAL_SERVICES', 'GENERAL_SERVICES_ACCOUNTING_AND_FINANCIAL_PLANNING', 'Planificación financiera, y servicios de impuestos y contabilidad'),
    ('GENERAL_SERVICES', 'GENERAL_SERVICES_AUTOMOTIVE', 'Cambios de aceite, lavado de autos, reparaciones y remolque'),
    ('GENERAL_SERVICES', 'GENERAL_SERVICES_CHILDCARE', 'Canguros y guardería'),
    ('GENERAL_SERVICES', 'GENERAL_SERVICES_CONSULTING_AND_LEGAL', 'Servicios de consultoría y legales'),
    ('GENERAL_SERVICES', 'GENERAL_SERVICES_EDUCATION', 'Escuela primaria, secundaria, escuelas profesionales y matrícula universitaria'),
    ('GENERAL_SERVICES', 'GENERAL_SERVICES_INSURANCE', 'Seguro para automóviles, hogar y atención médica'),
    ('GENERAL_SERVICES', 'GENERAL_SERVICES_POSTAGE_AND_SHIPPING', 'Servicios de correo, empaquetado y envío'),
    ('GENERAL_SERVICES', 'GENERAL_SERVICES_STORAGE', 'Servicios y facilidades de almacenamiento'),
    ('GENERAL_SERVICES', 'GENERAL_SERVICES_OTHER_GENERAL_SERVICES', 'Otros servicios diversos, incluyendo publicidad y almacenamiento en la nube');

-- Insertar categorías de GOVERNMENT_AND_NON_PROFIT
INSERT INTO PlaidCategories (PrimaryCategory, DetailedCategory, Description)
VALUES 
    ('GOVERNMENT_AND_NON_PROFIT', 'GOVERNMENT_AND_NON_PROFIT_DONATIONS', 'Donaciones benéficas, políticas y religiosas'),
    ('GOVERNMENT_AND_NON_PROFIT', 'GOVERNMENT_AND_NON_PROFIT_GOVERNMENT_DEPARTMENTS_AND_AGENCIES', 'Departamentos y agencias gubernamentales, como licencias de conducir y renovación de pasaportes'),
    ('GOVERNMENT_AND_NON_PROFIT', 'GOVERNMENT_AND_NON_PROFIT_TAX_PAYMENT', 'Pagos de impuestos, incluyendo impuestos sobre la renta y la propiedad'),
    ('GOVERNMENT_AND_NON_PROFIT', 'GOVERNMENT_AND_NON_PROFIT_OTHER_GOVERNMENT_AND_NON_PROFIT', 'Otras agencias gubernamentales y sin fines de lucro diversas');

-- Insertar categorías de TRANSPORTATION
INSERT INTO PlaidCategories (PrimaryCategory, DetailedCategory, Description)
VALUES 
    ('TRANSPORTATION', 'TRANSPORTATION_BIKES_AND_SCOOTERS', 'Alquiler de bicicletas y scooters'),
    ('TRANSPORTATION', 'TRANSPORTATION_GAS', 'Compras en una gasolinera'),
    ('TRANSPORTATION', 'TRANSPORTATION_PARKING', 'Tarifas y gastos de estacionamiento'),
    ('TRANSPORTATION', 'TRANSPORTATION_PUBLIC_TRANSIT', 'Transporte público, incluyendo ferrocarril y tren, autobuses y metro'),
    ('TRANSPORTATION', 'TRANSPORTATION_TAXIS_AND_RIDE_SHARES', 'Servicios de taxi y compartir viajes'),
    ('TRANSPORTATION', 'TRANSPORTATION_TOLLS', 'Gastos de peaje'),
    ('TRANSPORTATION', 'TRANSPORTATION_OTHER_TRANSPORTATION', 'Otros gastos de transporte diversos');
