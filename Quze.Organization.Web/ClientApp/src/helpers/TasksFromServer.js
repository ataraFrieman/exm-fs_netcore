
export function getTasks(){
    //array of objects and not object that contain array of objects
    return [
            {
                id: 1,
                title: [
                    {
                        first: "לוודא בדיקת דם בתוקף"
                    },
                    {
                        second: "לוודא התחייבות"
                    }
                ],
                name: "דוד אנגלברד",
                age: "55",
                serviceType: "צילום כליות",
                providerService: "ד''ר אלי ליפשיץ",
                date: "24/12/18 - 12:30",
                details: [
                    {
                        first: "להביא התחייבות"
                    },
                    {
                        second: "חסרה בדיקת דם בתוקף"
                    }
                ],
                email: "dodo@yahoo.com",
                phone: "050-5452390",
                gender: "גבר",
                isUrgent: true,
                noShow: 0
            },
            {
                id: "2",
                title: [
                    {
                        first: "לוודא שבא עם מלווה"
                    }
                ],
                name: "יעקב טורנטון",
                age: "67",
                serviceType: "צילום רנטגן",
                providerService: "ד''ר תמר בורנשטיין",
                date: "23/12/18 - 10:15",
                details: [
                    {
                        first: "צילום רנטגן"
                    },
                    {
                        second: "מושתל עם קוצב לב ,צריך לבוא עם מלווה"
                    }
                ],
                email: "torjack@gmail.com",
                phone: "054-6436372",
                gender: "גבר",
                isUrgent: false,
                noShow: 0
            },
            {
                id: "3",
                title: [
                    {
                        first: "לוודא הגעה"
                    }
                ],
                name: "אליס שירה",
                age: "47",
                serviceType: "צילום רנטגן",
                providerService: "ד''ר תמר בורנשטיין",
                date: "25/12/18 - 9:20",
                details: [
                    {
                        first: "לא ענתה למייל ולאסמס"
                    },
                    {
                        second: "צילום רנטגן"
                    }
                ],
                email: "shilor@walla.co.il",
                phone: "058-3490021",
                gender: "אישה",
                isUrgent: false,
                noShow: 0
            },
            {
                id: "4",
                title: [
                    {
                        first: "לוודא צום 12 שעות"
                    }
                ],
                name: "אלי מזרחי",
                age: "51",
                serviceType: "בדיקת MRI",
                providerService: "ד''ר יואב ארי",
                date: "23/12/18 - 14:45",
                details: [
                    {
                        first: "צום 12 שעות לפני"
                    },
                    {
                        second: "צילום ריאות"
                    }
                ],
                email: "elimiz@walla.co.il",
                phone: "052-5100201",
                gender: "גבר",
                isUrgent: false,
                noShow: 0
            },
            {
                id: "5",
                title: [
                    {
                        first: "לוודא שבא עם מלווה"
                    }
                ],
                name: "עוז רז",
                age: "13",
                serviceType: "בדיקת עיניים",
                providerService: "ד''ר חן גינאי",
                date: "25/12/18 - 11:05",
                details: [
                    {
                        first: "ליווי מבוגר"
                    }
                ],
                email: "shirSam@walla.co.il",
                phone: "052-5103501",
                gender: "גבר",
                isUrgent: false,
                noShow: 0
            },
            {
                id: "6",
                title: [
                    {
                        first: "סיכוי גבוה לאי הגעה"
                    }
                ],
                name: "אבי לוי",
                age: "45",
                serviceType: "בדיקת עיניים",
                providerService: "ד''ר מוחמד ז'אן",
                date: "27/12/18 - 14:00",
                details: [
                    {
                        first: "לזמן פגישה נוספת של כ- 10 דקות בשעה 14:00"
                    }
                ],
                email: "avilov@gmail.com",
                phone: "053-5421978",
                gender: "גבר",
                isUrgent: false,
                noShow: 1
            }
            // {
            //     id: "",
            //     title: [],
            //     name: "",
            //     age: "",
            //     serviceType: "",
            //     providerService: "",
            //     date: "",
            //     details: [],
            //     email: "",
            //     phone: "",
            //     gender: "",
            //     noShow: ""
            // }
        //]
    //}
    ];
}