# TPF-Coton



Modification pre-commit protection de gros volume


dans le dossier projet faire un clic droit -> inviter de commande -> git init.

afficher les fichiers cacher, trouver le dossier .git

et dans pre-commit.sample coller cette parti de code juste avant les 2 dernières lignes.



\# Refuser un commit dont la taille totale dépasse 99 Mo

max\_size=$((99 \* 1024 \* 1024)) # 99 Mo

total\_size=0



echo "🔍 Vérification de la taille totale des fichiers commités..."



files=$(git diff --cached --name-only --diff-filter=AM)



for file in $files; do

&nbsp;	if \[ -f "$file" ]; then

&nbsp;		size=$(stat -c%s "$file" 2>/dev/null || wc -c <"$file")

&nbsp;		total\_size=$((total\_size + size))

&nbsp;	fi

done



if \[ "$total\_size" -gt "$max\_size" ]; then

&nbsp;	echo "❌ Taille totale des fichiers commités : $total\_size octets (> 99 Mo)."

&nbsp;	echo "Le commit est bloqué. Veuillez retirer ou réduire certains fichiers."

&nbsp;	exit 1

fi



echo "✅ Taille totale des fichiers commités : $total\_size octets (sous la limite de 99 Mo)."



