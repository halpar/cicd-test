cd "${CONFIGURATION_BUILD_DIR}/${UNLOCALIZED_RESOURCES_FOLDER_PATH}/Frameworks/UnityFramework.framework/"
if [[ -d "Frameworks" ]]; then
    rm -fr Frameworks
    echo "Deleting frameworks folder process sucessful"
else
    echo "Deleting frameworks folder process failed"
fi
