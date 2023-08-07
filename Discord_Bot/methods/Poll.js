const {  EmbedBuilder } = require('discord.js');

function editEmbed(whichField, interaction) {
    // Get the username of the user that triggered the button
    const username = interaction.user.username;

    // Update the embed to add the username as the last value of the first field
    const oldEmbed = interaction.message.embeds[whichField];
console.log(oldEmbed);
    // Make a copy of the first field
    const updatedFirstField = { ...oldEmbed.fields[whichField] };

    // Remove the default "-" value from the first field if it exists
    if (updatedFirstField.value === "-") {
      updatedFirstField.value = "";
    }

    // Append the username to the value of the first field
    updatedFirstField.value += `\n${username}`;

    // Copy the rest of the fields from the old embed to the new embed
    const newEmbed = new EmbedBuilder()
      .setTitle(oldEmbed.title)
      .setDescription(oldEmbed.description)
      .setColor(oldEmbed.color)
      .setAuthor(oldEmbed.author)
      .setFooter(oldEmbed.footer)
      .setTimestamp(oldEmbed.setTimestamp)
      .spliceFields(whichField, whichField + 1, updatedFirstField); // Replace the first field with the updated one

    // Add the remaining fields from the old embed to the new embed
    for (let i = whichField + 1; i < oldEmbed.fields.length; i++) {
      const field = oldEmbed.fields[i];
      newEmbed.addFields([
        { name: field.name, value: field.value, inline: field.inline },
      ]);
    }

    return newEmbed;
}

module.exports = {
  editEmbed: editEmbed,
};
