<?php

echo 'Hello!' . "\n";

//* Variable, echo ja laskuesimerkki *//
$etunimi = 'Tuuli';
$sukunimi = 'Mantila';

echo "Nimeni on $etunimi $sukunimi." . "\n";
echo 'Tassa on PHP-alkeeni.' . "\n";

$numero = 24;
$esim = "numero";

echo 'Ikani on ' . $$esim . "." . "\n";
echo 'Tuplattuna se on ' . $numero * 2 . "." . "\n" . "\n";

//* Array ja foreach esimerkki *//
$yhteystiedot = array(
    'nimi' => "Tuuli Mantila",
    'katuosoite' => "Mansikkamaantie 6",
    'postitoimipaikka' => "Kultasaari",
    'puhelin' => "0202020202",
    'sposti' => "mansikka@mansikka.fi",
);

print_r($yhteystiedot);

echo "\n";

echo 'Yhteystietoni:' . "\n";

foreach($yhteystiedot as $arvo){
    echo $arvo . "\n";
}

echo "\n";

//*If Else esimerkki*//
$vertailu = 5;

echo 'Onko luku viisi pienempi vai suurempi kuin kymmenen?' . "\n";

if ($vertailu > 10) {
    echo 'Luku on suurempi kuin 10.' . "\n" . "\n";
} else {
    echo 'Luku on pienempi kuin 10.' . "\n" . "\n";
}

//*Switch esimerkki*//
echo 'Muuttujan $numero arvo on: ';

switch($numero){
    case 20:
        echo 'Kaksikymmenta' . "\n" . "\n";
        break;
    case 21:
        echo 'Kaksikymmentayksi' . "\n" . "\n";
        break;
    case 22:
        echo 'Kaksikymmentakaksi' . "\n" . "\n";
        break;
    case 23:
        echo 'Kaksikymmentakolme' . "\n" . "\n";
        break;
    case 24:
        echo 'Kaksikymmentanelja' . "\n" . "\n";
        break;
    default:
        echo 'Ei tietoa' . "\n" . "\n";
        break;
}

//* For esimerkki *//
echo 'Tulostetaan parittomat luvut lukuun 15 asti.' . "\n" . "\n";

for($i=1;$i<=15;$i=$i+2) {
    echo "$i" . "\n";
}

echo "\n";

//* While esimerkki kayttaen arrayta *//
$numerojono = array(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);

echo 'Tulostetaan koko numerojono lukuun 10 asti.' . "\n" . "\n";

$i = 0;

while ($numerojono[$i]){
    echo $numerojono[$i] . "\n";
    $i++;
}

?>
