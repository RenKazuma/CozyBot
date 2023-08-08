const { EmbedBuilder } = require("discord.js");

async function editEmbed(whichField, interaction) {
  const username = interaction.user.username;

  // Update the embed to add the username as the last value of the specified field
  const oldEmbed = interaction.message.embeds[0];

  // Make a copy of the specified field
  var updatedField = { ...oldEmbed.fields[whichField] };

  var multipleChoiceValue;
  if (oldEmbed.footer.text.includes(process.env.MultipleChoiceFalse))
    multipleChoiceValue = false;
  else multipleChoiceValue = true;

  if (!multipleChoiceValue) {
    for (let i = 0; i < oldEmbed.fields.length; i++) {
      const field = oldEmbed.fields[i];

      if (field.value.includes(username)) {
        oldEmbed.fields[i].value = oldEmbed.fields[i].value.replace(
          username,
          ""
        );
        oldEmbed.fields[i] = checkNewlineCount(oldEmbed.fields[i]);
        break;
      }
    }
  }

  // Remove the default "-" value from the specified field if it exists
  if (updatedField.value === "-") {
    updatedField.value = "";
  }

  // Check if the username is already present in the specified field
  if (updatedField.value.includes(username)) {
    // Return a message indicating that the user has already voted
    console.log(updatedField.value);
    updatedField.value = updatedField.value.replace(`${username}`, "");
  } else {
    // Append the username to the value of the specified field
    updatedField.value += `\n${username}`;
  }

  updatedField = checkNewlineCount(updatedField);

  // Copy the rest of the fields from the old embed to the new embed
  const newEmbed = new EmbedBuilder(oldEmbed).spliceFields(
    whichField,
    1,
    updatedField
  ); // Replace the specified field with the updated one

  return {
    newEmbed: newEmbed,
    name: updatedField.name.slice(0, -3),
  };
}

function checkNewlineCount(updatedField) {
  const usernamesArray = updatedField.value.split('\n');
  
  const uniqueUsernames = new Set(usernamesArray.filter(username => username.trim() !== ''));
  console.log(uniqueUsernames)
  const numberOfUniqueUsers = uniqueUsernames.size;
  console.log(numberOfUniqueUsers);

  if (numberOfUniqueUsers >= 1){
    updatedField.name = removeNumberFromEnd(updatedField.name);
  } 

  if (numberOfUniqueUsers !== 0) {
    updatedField.name = `${updatedField.name} (${numberOfUniqueUsers})`;
    return updatedField;
  }

  updatedField.name = removeNumberFromEnd(updatedField.name);
  updatedField.value = "-";

  return updatedField;
}

function removeNumberFromEnd(inputString) {
  const numberPattern = /\(\d+\)$/;
  return inputString.replace(numberPattern, "").trim();
}


module.exports = {
  editEmbed: editEmbed,
};