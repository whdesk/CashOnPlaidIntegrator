namespace PlaidApi.Constants
{
    public static class PaymentCapacityRules
    {
        // Categorías de Ingresos Recurrentes
        public static readonly string[] IngresosRecurrentes = [
            PlaidCategories.INCOME_WAGES, // Ingresos de salarios, trabajos de economía colaborativa y propinas ganadas
            PlaidCategories.INCOME_RETIREMENT_PENSION, // Ingresos de pagos de pensiones
            PlaidCategories.INCOME_UNEMPLOYMENT, // Ingresos de beneficios por desempleo, incluyendo seguro de desempleo y atención médica
            PlaidCategories.RENT_AND_UTILITIES_RENT, // Pago de alquiler recibido
            PlaidCategories.INCOME_OTHER_INCOME // Otros ingresos diversos, incluyendo alimentos, seguridad social, manutención y alquiler
        ];

        // Categorías de Ingresos Puntuales a Excluir
        public static readonly string[] IngresosPuntualesExcluir = [
            PlaidCategories.INCOME_TAX_REFUND, // Ingresos de reembolsos de impuestos
            PlaidCategories.INCOME_DIVIDENDS, // Dividendos de cuentas de inversión
            PlaidCategories.INCOME_INTEREST_EARNED // Ingresos de intereses en cuentas de ahorros
            // Agrega aquí otras categorías de ingresos puntuales si existen (ej: PlaidCategories.INCOME_LOTTERY_PRIZE, PlaidCategories.INCOME_GIFTS)
        ];

        // Categorías de Transferencias a Excluir
        public static readonly string[] TransferenciasExcluir = [
            PlaidCategories.TRANSFER_IN_ACCOUNT_TRANSFER, // Transferencias generales desde otra cuenta
            PlaidCategories.TRANSFER_IN_CASH_ADVANCES_AND_LOANS, // Préstamos y avances en efectivo depositados en una cuenta bancaria
            PlaidCategories.TRANSFER_IN_INVESTMENT_AND_RETIREMENT_FUNDS, // Transferencias entrantes a una cuenta de inversión o retiro
            PlaidCategories.TRANSFER_IN_SAVINGS, // Transferencias entrantes a una cuenta de ahorros
            PlaidCategories.TRANSFER_IN_OTHER_TRANSFER_IN, // Otras transacciones entrantes diversas
            PlaidCategories.TRANSFER_IN_DEPOSIT // Depósitos de efectivo, cheques y cajeros automáticos en una cuenta bancaria
        ];

        // Categorías de Egresos Recurrentes
        public static readonly string[] EgresosRecurrentes = [
            PlaidCategories.RENT_AND_UTILITIES_RENT, // Pago de alquiler
            PlaidCategories.RENT_AND_UTILITIES_GAS_AND_ELECTRICITY, // Servicios de gas y electricidad
            PlaidCategories.RENT_AND_UTILITIES_INTERNET_AND_CABLE, // Servicios de internet y cable
            PlaidCategories.RENT_AND_UTILITIES_TELEPHONE, // Servicio telefónico
            PlaidCategories.RENT_AND_UTILITIES_WATER, // Servicio de agua
            PlaidCategories.RENT_AND_UTILITIES_OTHER_UTILITIES, // Otros servicios públicos
            PlaidCategories.RENT_AND_UTILITIES_SEWAGE_AND_WASTE_MANAGEMENT, // Gestión de aguas residuales y basura
            PlaidCategories.LOAN_PAYMENTS_CAR_PAYMENT, // Pago de préstamo de auto
            PlaidCategories.LOAN_PAYMENTS_CREDIT_CARD_PAYMENT, // Pago de tarjeta de crédito
            PlaidCategories.LOAN_PAYMENTS_MORTGAGE_PAYMENT, // Pago de hipoteca
            PlaidCategories.LOAN_PAYMENTS_PERSONAL_LOAN_PAYMENT, // Pago de préstamo personal
            PlaidCategories.LOAN_PAYMENTS_STUDENT_LOAN_PAYMENT, // Pago de préstamo estudiantil
            PlaidCategories.LOAN_PAYMENTS_OTHER_PAYMENT, // Otros pagos de préstamos
            PlaidCategories.GENERAL_SERVICES_EDUCATION, // Gastos educativos
            PlaidCategories.GENERAL_SERVICES_INSURANCE // Seguros
        ];

        // Categorías de Egresos Variables a Excluir
        public static readonly string[] EgresosVariablesExcluir = [
            // Restaurantes y comida
            PlaidCategories.FOOD_AND_DRINK_RESTAURANT, // Restaurantes
            PlaidCategories.FOOD_AND_DRINK_BEER_WINE_AND_LIQUOR, // Tiendas de cerveza, vino y licores
            PlaidCategories.FOOD_AND_DRINK_COFFEE, // Cafeterías
            PlaidCategories.FOOD_AND_DRINK_FAST_FOOD, // Comida rápida
            PlaidCategories.FOOD_AND_DRINK_GROCERIES, // Supermercados y abarrotes
            PlaidCategories.FOOD_AND_DRINK_VENDING_MACHINES, // Máquinas expendedoras
            PlaidCategories.FOOD_AND_DRINK_OTHER_FOOD_AND_DRINK, // Otros gastos de comida y bebida
            // Entretenimiento
            PlaidCategories.ENTERTAINMENT_CASINOS_AND_GAMBLING, // Apuestas, casinos y apuestas deportivas
            PlaidCategories.ENTERTAINMENT_MUSIC_AND_AUDIO, // Música y servicios de streaming de audio
            PlaidCategories.ENTERTAINMENT_SPORTING_EVENTS_AMUSEMENT_PARKS_AND_MUSEUMS, // Eventos deportivos, museos, parques de diversiones
            PlaidCategories.ENTERTAINMENT_TV_AND_MOVIES, // Streaming de películas y cines
            PlaidCategories.ENTERTAINMENT_VIDEO_GAMES, // Videojuegos
            PlaidCategories.ENTERTAINMENT_OTHER_ENTERTAINMENT, // Vida nocturna y entretenimiento para adultos
            // Compras personales
            PlaidCategories.PERSONAL_CARE_GYMS_AND_FITNESS_CENTERS, // Gimnasios y fitness
            PlaidCategories.PERSONAL_CARE_HAIR_AND_BEAUTY, // Peluquería y belleza
            PlaidCategories.PERSONAL_CARE_LAUNDRY_AND_DRY_CLEANING, // Lavandería y tintorería
            PlaidCategories.PERSONAL_CARE_OTHER_PERSONAL_CARE, // Otros cuidados personales
            // Transporte
            PlaidCategories.TRANSPORTATION_BIKES_AND_SCOOTERS, // Bicicletas y scooters
            PlaidCategories.TRANSPORTATION_GAS, // Gasolina
            PlaidCategories.TRANSPORTATION_PARKING, // Estacionamiento
            PlaidCategories.TRANSPORTATION_PUBLIC_TRANSIT, // Transporte público
            PlaidCategories.TRANSPORTATION_TAXIS_AND_RIDE_SHARES, // Taxis y apps de transporte
            PlaidCategories.TRANSPORTATION_TOLLS, // Peajes
            PlaidCategories.TRANSPORTATION_OTHER_TRANSPORTATION, // Otros gastos de transporte
            // Mejoras del hogar
            PlaidCategories.HOME_IMPROVEMENT_FURNITURE, // Muebles
            PlaidCategories.HOME_IMPROVEMENT_HARDWARE, // Ferretería
            PlaidCategories.HOME_IMPROVEMENT_REPAIR_AND_MAINTENANCE, // Reparaciones y mantenimiento
            PlaidCategories.HOME_IMPROVEMENT_SECURITY, // Seguridad del hogar
            PlaidCategories.HOME_IMPROVEMENT_OTHER_HOME_IMPROVEMENT, // Otras mejoras del hogar
            // Salud y médicos
            PlaidCategories.MEDICAL_DENTAL_CARE, // Cuidado dental
            PlaidCategories.MEDICAL_EYE_CARE, // Cuidado de la vista
            PlaidCategories.MEDICAL_NURSING_CARE, // Enfermería
            PlaidCategories.MEDICAL_PHARMACIES_AND_SUPPLEMENTS, // Farmacias y suplementos
            PlaidCategories.MEDICAL_PRIMARY_CARE, // Atención primaria
            PlaidCategories.MEDICAL_VETERINARY_SERVICES, // Servicios veterinarios
            PlaidCategories.MEDICAL_OTHER_MEDICAL, // Otros gastos médicos
            // Donaciones y gobierno
            PlaidCategories.GOVERNMENT_AND_NON_PROFIT_DONATIONS, // Donaciones
            PlaidCategories.GOVERNMENT_AND_NON_PROFIT_GOVERNMENT_DEPARTMENTS_AND_AGENCIES, // Pagos a entidades gubernamentales
            PlaidCategories.GOVERNMENT_AND_NON_PROFIT_TAX_PAYMENT, // Pago de impuestos
            PlaidCategories.GOVERNMENT_AND_NON_PROFIT_OTHER_GOVERNMENT_AND_NON_PROFIT // Otros pagos a gobierno y ONGs
        ];

        // Rangos de Ratio de Capacidad de Pago
        public enum PaymentCapacityLevel
        {
            Buena = 1, // Ratio > 1.5
            Aceptable = 2, // 1.2 ≤ Ratio ≤ 1.5
            Baja = 3 // Ratio ≤ 1.19
        }

        public static PaymentCapacityLevel GetPaymentCapacityLevel(decimal ratio)
        {
            if (ratio > 1.5m) return PaymentCapacityLevel.Buena;
            if (ratio >= 1.2m) return PaymentCapacityLevel.Aceptable;
            return PaymentCapacityLevel.Baja;
        }
    }
}
