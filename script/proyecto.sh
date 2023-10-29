#!/bin/bash

#Comando para ejecutar el proyecto Moogle
run() {
  if [[ "$OSTYPE" == "linux-gnu"* ]]; 
  then
  dotnet run 
  else
  dotnet run
fi
}

#Comando para compilar el informe en pdf
report() {
   pdflatex -output-directory=Informe Informe/informe.tex
   pdflatex -output-directory=Informe Informe/informe.tex
 echo ""
 echo "El informe.pdf ha sido generado"
}

#Comando para compilar la presentacion en pdf
slides() {
 pdflatex -output-directory=Presentacion Presentacion/presentacion.tex
 pdflatex -output-directory=Presentacion Presentacion/presentacion.tex
 echo ""
 echo "La presentacion.pdf ha sido generada"
}

#Comando para eliminar los archivos innecesarios producidos durante la compilacion
clean() {
  cd Informe
  rm -f informe.pdf informe.aux informe.fls informe.log informe.fdb_latexmk informe.out informe.synctex.gz informe.toc
  cd ..
  if [ -r  "bin" ]
  then
    rm -r bin obj
  fi
  echo "Limpieza terminada"
}

#Comando para mostrar el informe y compilarlo en caso de no haberlo hecho anteriormente
show_report() {
  if [ ! -f  "Informe/informe.pdf" ]
  then 
    report;
  fi

  if [ -z "$1" ]
  then
     if [[ "$OSTYPE" == "linux-gnu"* ]]; 
     then
     xdg-open Informe/informe.pdf
     elif [[ "$OSTYPE" == "darwin"* ]];
     then
     open Informe/informe.pdf
     else
     start Informe/informe.pdf
     fi
  else
    $1 Informe/informe.pdf
  fi
}

#Comando de informacion
info() {
  echo "El script consta de los siguientes comandos: "
  echo ""
  echo "run         -Para ejecutar el proyecto HULK"
  echo "report      -Para compilar y generar el pdf del proyecto latex del informe"
  echo "show_report -Para visualizar el informe y si el fichero pdf  no ha sido creado entonces generarlo"
  echo "clean       -Para eliminar todos los ficheros auxiliares que no forman parte del contenido del repositorio"
  echo "info        -Para visualizar esta información"
  echo ""
}

if [[ $# -eq 0 ]];
then
info;
exit 0
fi
cd ..
"$@"